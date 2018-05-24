#include "stdafx.h"
#include "_pri_mail.h"

#ifdef _DEBUG
#pragma comment(lib, "jwsmtpd.lib")
#else
#pragma comment(lib, "jwsmtp.lib")
#endif

namespace SSUtils
{
	namespace Mail
	{
		jwsmtp::mailer generateMessageMailer(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & title, const std::string & content, const CharType charType)
		{
			String::Converter converter(charType, CharType::UTF8);

			return jwsmtp::mailer("", senderMail.c_str(),
				converter(title).c_str(), converter(content).c_str(),
				senderServer.c_str(), jwsmtp::mailer::SMTP_PORT, false);
		}

		jwsmtp::mailer generateHTMLMailer(const std::string & senderMail, const std::string &senderPassword, const std::string &senderServer, const std::string &title, const std::string &content, const CharType charType)
		{
			String::Converter converter(charType, CharType::UTF8);

			jwsmtp::mailer mailer("", senderMail.c_str(),
				converter(title).c_str(), "",
				senderServer.c_str(), jwsmtp::mailer::SMTP_PORT, false);
			mailer.setmessageHTML(converter(content).c_str());

			return mailer;
		}

		std::pair<bool, std::string> sendMail(const std::string &senderMail, const std::string &senderPassword, jwsmtp::mailer &mailer)
		{
			static const std::string SendMailOKCode("250");

			mailer.authtype(jwsmtp::mailer::authtype::PLAIN);

			mailer.username(senderMail);
			mailer.password(senderPassword);

			mailer.send();
			return std::make_pair(mailer.response().find(SendMailOKCode) == 0, String::toLocal(CharType::UTF8, mailer.response()));
		}

		const std::map<MessageType, MailerGenerator> MailerGenerators = 
		{
			std::make_pair(MessageType::Message, generateMessageMailer),
			std::make_pair(MessageType::Html, generateHTMLMailer)
		};
	};
};
