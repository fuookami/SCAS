#pragma once

#include "DataUtils.h"

#include <string>
#include <utility>

namespace SSUtils
{
	namespace Encryption
	{
		namespace RSA
		{
			/*!
			*	\������:   	generateKey
			*	\����: 		unsigned int keyLength						��Կ����
			*	\����: 		const std::string & seed					���ӣ����ڹ���������أ��������棩�����������
			*	\����ֵ:   	std::pair<std::string, std::string>	RSA��Կ��˽Կ��
			*	\˵��: 		����RSA��Կ��˽Կ�ԣ�������֤�������������������������롢����
			*/
			std::pair<std::string, std::string> generateKey(const uint32 keyLength, const std::string &seed = Data::toString(Data::generateRandomBlock()));

			/*!
			*	\������:   	encrypt
			*	\����: 		const std::string & publicKey	RSA��Կ�����ڼ�����Ϣ
			*	\����: 		const std::string & origin		��������Ϣ
			*	\����: 		const std::string seed			���ӣ����ڹ���������أ��������棩�����������
			*	\����ֵ:   	std::string						����
			*	\˵��: 		ʹ��RSA��Կ����Ϣ���м���
			*/
			std::string encrypt(const std::string &publicKey, const std::string &origin, const std::string &seed = Data::toString(Data::generateRandomBlock()));

			struct encrypter
			{
				encrypter(const std::string &_seed = Data::toString(Data::generateRandomBlock()));
				encrypter(const std::string &_publicKey, const std::string &_seed = Data::toString(Data::generateRandomBlock()));
				encrypter(const encrypter &ano) = default;
				encrypter(encrypter &&ano) = default;
				encrypter &operator=(const encrypter &rhs) = default;
				encrypter &operator=(encrypter &&rhs) = default;
				~encrypter(void) = default;

				std::string operator()(const std::string &origin);

				std::string publicKey;
				std::string seed;
			};

			/*!
			*	\������:   	decrypt
			*	\����: 		const std::string & privateKey	RSA˽Կ�����ڽ�������
			*	\����: 		const std::string & cipher		����
			*	\����ֵ:   	std::string						���ܺ�����
			*	\˵��: 		ʹ��RSA˽Կ�����Ľ��н���
			*/
			std::string decrypt(const std::string &privateKey, const std::string &cipher);

			struct decrypter
			{
				decrypter(void) = default;
				decrypter(const std::string &_privateKey);
				decrypter(const decrypter &ano) = default;
				decrypter(decrypter &&ano) = default;
				decrypter &operator=(const decrypter &rhs) = default;
				decrypter &operator=(decrypter &&rhs) = default;
				~decrypter(void) = default;

				std::string operator()(const std::string &cipher);

				std::string privateKey;
			};

			/*!
			*	\������:   	sign
			*	\����: 		const std::string & privateKey	RSA˽Կ
			*	\����: 		const std::string & msg			ԭʼ����
			*	\����ֵ:   	std::string						��ǩ������
			*	\˵��: 		ʹ��RSA˽Կǩ���ļ�
			*/
			std::string sign(const std::string &privateKey, const std::string &msg);

			struct signer
			{
				signer(void) = default;
				signer(const std::string &_privateKey);
				signer(const signer &ano) = default;
				signer(signer &&ano) = default;
				signer &operator=(const signer &rhs) = default;
				signer &operator=(signer &&rhs) = default;
				~signer(void) = default;

				std::string operator()(const std::string &msg);

				std::string privateKey;
			};

			/*!
			*	\������:   	verify
			*	\����: 		const std::string & publicKey	RSA��Կ
			*	\����: 		const std::string & msg			ԭʼ����
			*	\����: 		const std::string & signature	��ǩ������
			*	\����ֵ:   	const bool						�Ƿ�ƥ�䣬true��ʾƥ�䣬false��ʾ��ƥ��
			*	\˵��: 		ʹ��RSA��Կ��֤�ļ�ǩ��
			*/
			const bool verify(const std::string &publicKey, const std::string &msg, const std::string &signature);

			struct verifier
			{
				verifier(void) = default;
				verifier(const std::string &_publicKey);
				verifier(const verifier &ano) = default;
				verifier(verifier &&ano) = default;
				verifier &operator=(const verifier &rhs) = default;
				verifier &operator=(verifier &&rhs) = default;
				~verifier(void) = default;

				const bool operator()(const std::string &msg, const std::string &signature);

				std::string publicKey;
			};
		};
	};
};
