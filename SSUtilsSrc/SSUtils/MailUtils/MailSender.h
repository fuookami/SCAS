#pragma once

#include "_pri_mail_global.h"
#include "..\StringUtils.h"
#include <memory>

namespace SSUtils
{
	namespace Mail
	{
		struct SSUtils_API_DECLSPEC Sender
		{
			std::string senderMail;
			std::string senderPassword;
			std::string senderServer;
			std::vector<std::string> receiverMails;

			Sender(const std::string &_senderMail, const std::string &_senderPassword, const std::string &_senderServer, const std::string _receiverMail);
			Sender(const std::string &_senderMail, const std::string &_senderPassword, const std::string &_senderServer, const std::vector<std::string> &_receiverMails);
			Sender(const Sender &ano) = default;
			Sender(Sender &&ano) = default;
			Sender &operator=(const Sender &rhs) = default;
			Sender &operator=(Sender &&rhs) = default;
			~Sender(void) = default;

			std::pair<bool, std::string> operator()(const MessageType type, const std::string & title, const std::string & content, const CharType charType = String::LocalCharType());
		};
	};
};
