#pragma once

#include <vector>
#include <string>
#include <utility>
#include <jwsmtp/jwsmtp.h>

namespace MailUtils
{
	std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string &title, const std::string &content);
	std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string> & receiverMails, const std::string &title, const std::string &content);

	std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string &title, const std::string &content);
	std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string> & receiverMails, const std::string &title, const std::string &content);

	std::pair<bool, std::string> __sendMail(const std::string & senderMail, const std::string & senderPassword, jwsmtp::mailer &mail);
};
