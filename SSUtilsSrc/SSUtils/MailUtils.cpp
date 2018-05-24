#include "stdafx.h"
#include "MailUtils.h"

namespace SSUtils
{
	namespace Mail
	{
		std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string & title, const std::string & content, const CharType charType)
		{
			Sender sender(senderMail, senderPassword, senderServer, receiverMail);
			return sender(MessageType::Message, title, content, charType);
		}

		std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string>& receiverMails, const std::string & title, const std::string & content, const CharType charType)
		{
			Sender sender(senderMail, senderPassword, senderServer, receiverMails);
			return sender(MessageType::Message, title, content, charType);
		}

		std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string & title, const std::string & content, const CharType charType)
		{
			Sender sender(senderMail, senderPassword, senderServer, receiverMail);
			return sender(MessageType::Html, title, content, charType);
		}

		std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string>& receiverMails, const std::string & title, const std::string & content, const CharType charType)
		{
			Sender sender(senderMail, senderPassword, senderServer, receiverMails);
			return sender(MessageType::Html, title, content, charType);
		}
	};
};
