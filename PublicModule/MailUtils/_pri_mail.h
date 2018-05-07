#pragma once

#include "_pri_mail_global.h"
#include "StringUtils.h"
#include <vector>
#include <string>
#include <utility>
#include <functional>
#include <map>
#include <jwsmtp/jwsmtp.h>

namespace SSUtils
{
	namespace Mail
	{
		jwsmtp::mailer generateMessageMailer(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & title, const std::string & content, const CharType charType = String::LocalCharType);
		jwsmtp::mailer generateHTMLMailer(const std::string & senderMail, const std::string &senderPassword, const std::string &senderServer, const std::string &title, const std::string &content, const CharType charType = String::LocalCharType);

		std::pair<bool, std::string> sendMail(const std::string &senderMail, const std::string &senderPassword, jwsmtp::mailer &mailer);

		using MailerGenerator = std::function<jwsmtp::mailer(const std::string &, const std::string &, const std::string &, const std::string &, const std::string &, const CharType)>;
		extern const std::map<MessageType, MailerGenerator> MailerGenerators;
	};
};
