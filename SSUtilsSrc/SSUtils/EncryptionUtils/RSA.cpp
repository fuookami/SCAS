#include "stdafx.h"
#include "RSA.h"
#include "_pri_encryption_global.h"
#include "..\StringUtils.h"
#include <cryptopp/rsa.h>
#include <cryptopp/hex.h>
#include <numeric>

namespace SSUtils
{
	namespace Encryption
	{
		namespace RSA
		{
			std::pair<std::string, std::string> generateKey(const uint32 keyLength, const std::string & seed)
			{
				std::pair<std::string, std::string> ret;
				std::string &privateKey(ret.first);
				std::string &publicKey(ret.second);

				CryptoPP::RandomPool randPool;
				randPool.IncorporateEntropy((const byte *)seed.c_str(), seed.size());

				// 生成私钥
				CryptoPP::RSAES_OAEP_SHA_Decryptor priDecryptor(randPool, keyLength);
				CryptoPP::HexEncoder privateEncoder(new CryptoPP::StringSink(privateKey));
				priDecryptor.DEREncode(privateEncoder);
				privateEncoder.MessageEnd();

				// 使用刚才生成的私钥生成公钥
				CryptoPP::RSAES_OAEP_SHA_Encryptor pubEncryptor(priDecryptor);
				CryptoPP::HexEncoder publicEncoder(new CryptoPP::StringSink(publicKey));
				pubEncryptor.DEREncode(publicEncoder);
				publicEncoder.MessageEnd();

				return ret;
			}

			std::string encrypt(const std::string & publicKey, const std::string & origin, const std::string & seed)
			{
				CryptoPP::StringSource PublicKeySrc(publicKey, true, new CryptoPP::HexDecoder);
				CryptoPP::RSAES_OAEP_SHA_Encryptor pubEncryptor(PublicKeySrc);

				CryptoPP::RandomPool randPool;
				randPool.IncorporateEntropy((const byte *)seed.c_str(), seed.size());

				// 每次加密有长度限制，需要分块加密后拼接
				uint32 maxMsgLen(static_cast<uint32>(pubEncryptor.FixedMaxPlaintextLength()));
				std::vector<std::string> subCiphers;

				for (auto currIt(origin.cbegin()), edIt(origin.cend()); currIt != edIt; )
				{
					uint32 thisLen(static_cast<uint32>(edIt - currIt));
					thisLen = thisLen > maxMsgLen ? maxMsgLen : thisLen;
					auto nextIt(currIt + thisLen);

					std::string subCipher;
					CryptoPP::StringSource(std::string(currIt, nextIt),
						new CryptoPP::PK_EncryptorFilter(randPool, pubEncryptor,
							new CryptoPP::HexEncoder(new CryptoPP::StringSink(subCipher))));

					subCiphers.emplace_back(subCipher);
					currIt = nextIt;
				}

				return std::accumulate(subCiphers.cbegin(), subCiphers.cend(), std::string(""));
			}

			std::string decrypt(const std::string & privateKey, const std::string & cipher)
			{
				CryptoPP::StringSource priKeySrc(privateKey, new CryptoPP::HexDecoder());
				CryptoPP::RSAES_OAEP_SHA_Decryptor priDecryptor(priKeySrc);

				// 把密文解密成十六进制码
				uint32 maxCiphertextLen(static_cast<uint32>(priDecryptor.FixedCiphertextLength()) * 2);
				std::vector<std::string> subDecrypts;

				for (auto currIt(cipher.cbegin()), edIt(cipher.cend()); currIt != edIt; )
				{
					uint32 thisLen(static_cast<uint32>(edIt - currIt));
					thisLen = thisLen > maxCiphertextLen ? maxCiphertextLen : thisLen;
					auto nextIt(currIt + thisLen);

					std::string subDecrypt;
					CryptoPP::StringSource(std::string(currIt, nextIt), true,
						new CryptoPP::HexDecoder(
							new CryptoPP::PK_DecryptorFilter(globalRNG, priDecryptor,
								new CryptoPP::StringSink(subDecrypt))));

					subDecrypts.emplace_back(subDecrypt);
					currIt += thisLen;
				}

				return Data::toString(Data::fromHexString(std::accumulate(subDecrypts.cbegin(), subDecrypts.cend(), std::string(""))));
			}

			std::string sign(const std::string & privateKey, const std::string & msg)
			{
				CryptoPP::StringSource privSrc(privateKey, true, new CryptoPP::HexDecoder());
				CryptoPP::RSASS<CryptoPP::PKCS1v15, CryptoPP::SHA>::Signer priSinger(privSrc);
				std::string signature;
				CryptoPP::StringSource(msg, true,
					new CryptoPP::SignerFilter(globalRNG, priSinger,
						new CryptoPP::HexEncoder(
							new CryptoPP::StringSink(signature))));

				return signature;
			}

			const bool verify(const std::string & publicKey, const std::string & msg, const std::string & signature)
			{
				CryptoPP::StringSource pubSrc(publicKey, true, new CryptoPP::HexDecoder());
				CryptoPP::RSASS<CryptoPP::PKCS1v15, CryptoPP::SHA>::Verifier pubVerifier(pubSrc);
				CryptoPP::StringSource signatureSrc(signature, true, new CryptoPP::HexDecoder());
				if (signatureSrc.MaxRetrievable() != pubVerifier.SignatureLength())
				{
					return false;
				}

				CryptoPP::SecByteBlock block(pubVerifier.SignatureLength());
				signatureSrc.Get(block, block.size());

				return true;
			}

			encrypter::encrypter(const std::string & _seed)
				: encrypter("", _seed)
			{
			}

			encrypter::encrypter(const std::string & _publicKey, const std::string & _seed)
				: publicKey(_publicKey), seed(_seed)
			{
			}

			std::string encrypter::operator()(const std::string & origin)
			{
				return publicKey.empty() ? String::EmptyString() : encrypt(publicKey, origin, seed);
			}

			decrypter::decrypter(const std::string & _privateKey)
				: privateKey(_privateKey)
			{
			}

			std::string decrypter::operator()(const std::string & cipher)
			{
				return privateKey.empty() ? String::EmptyString() : decrypt(privateKey, cipher);
			}

			signer::signer(const std::string & _privateKey)
				: privateKey(_privateKey)
			{
			}

			std::string signer::operator()(const std::string & msg)
			{
				return privateKey.empty() ? String::EmptyString() : sign(privateKey, msg);
			}

			verifier::verifier(const std::string & _publicKey)
				: publicKey(_publicKey)
			{
			}

			const bool verifier::operator()(const std::string & msg, const std::string & signature)
			{
				return publicKey.empty() ? false : verify(publicKey, msg, signature);
			}
};
	};
};
