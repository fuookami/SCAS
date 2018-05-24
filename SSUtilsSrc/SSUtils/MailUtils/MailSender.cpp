#include "stdafx.h"
#include "MailSender.h"
#include "_pri_mail.h"

namespace SSUtils
{
	namespace Mail
	{
		Sender::Sender(const std::string & _senderMail, const std::string & _senderPassword, const std::string & _senderServer, const std::string _receiverMail)
			: senderMail(_senderMail), senderPassword(_senderPassword), senderServer(_senderServer), receiverMails(1, _receiverMail)
		{
		}

		Sender::Sender(const std::string & _senderMail, const std::string & _senderPassword, const std::string & _senderServer, const std::vector<std::string>& _receiverMails)
			: senderMail(_senderMail), senderPassword(_senderPassword), senderServer(_senderServer), receiverMails(_receiverMails)
		{
		}

		std::pair<bool, std::string> Sender::operator()(const MessageType type, const std::string & title, const std::string & content, const CharType charType)
		{
			const auto &mailGenerator(MailerGenerators.find(type)->second);
			auto mailer(mailGenerator(senderMail, senderPassword, senderServer, title, content, charType));

			for (const auto &receiverMail : receiverMails)
			{
				mailer.addrecipient(receiverMail);
			}

			return sendMail(senderMail, senderPassword, mailer);
		}
	};
};
