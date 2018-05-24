#pragma once

#include "MailUtils/MailSender.h"
#include "StringUtils.h"
#include <string>
#include <utility>

namespace SSUtils
{
	namespace Mail
	{
		SSUtils_API_DECLSPEC std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string &title, const std::string &content, const CharType charType = String::LocalCharType());
		SSUtils_API_DECLSPEC std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string> & receiverMails, const std::string &title, const std::string &content, const CharType charType = String::LocalCharType());

		SSUtils_API_DECLSPEC std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string &title, const std::string &content, const CharType charType = String::LocalCharType());
		SSUtils_API_DECLSPEC std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string> & receiverMails, const std::string &title, const std::string &content, const CharType charType = String::LocalCharType());
	};
};
