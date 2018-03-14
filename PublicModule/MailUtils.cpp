#include "MailUtils.h"
#include "StringConvertUtils.h"
#include <jwsmtp/jwsmtp.h>

namespace MailUtils
{
	std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string & title, const std::string & content)
	{
		using namespace StringConvertUtils;

		jwsmtp::mailer mail(receiverMail.c_str(), senderMail.c_str(),
			fromLocal<CharType::UTF8>(title).c_str(), fromLocal<CharType::UTF8>(content).c_str(),
			senderServer.c_str(), jwsmtp::mailer::SMTP_PORT, false);

		return __sendMail(senderMail, senderPassword, mail);
	}

	std::pair<bool, std::string> sendMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string>& receiverMails, const std::string & title, const std::string & content)
	{
		using namespace StringConvertUtils;

		jwsmtp::mailer mail("", senderMail.c_str(),
			fromLocal<CharType::UTF8>(title).c_str(), fromLocal<CharType::UTF8>(content).c_str(),
			senderServer.c_str(), jwsmtp::mailer::SMTP_PORT, false);

		for (const auto &receiverMail : receiverMails)
		{
			mail.addrecipient(receiverMail);
		}

		return __sendMail(senderMail, senderPassword, mail);
	}

	std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::string & receiverMail, const std::string & title, const std::string & content)
	{
		using namespace StringConvertUtils;

		jwsmtp::mailer mail(receiverMail.c_str(), senderMail.c_str(),
			fromLocal<CharType::UTF8>(title).c_str(), "",
			senderServer.c_str(), jwsmtp::mailer::SMTP_PORT, false);
		mail.setmessageHTML(fromLocal<CharType::UTF8>(content).c_str());
		
		return __sendMail(senderMail, senderPassword, mail);
	}

	std::pair<bool, std::string> sendHTMLMail(const std::string & senderMail, const std::string & senderPassword, const std::string & senderServer, const std::vector<std::string>& receiverMails, const std::string & title, const std::string & content)
	{
		using namespace StringConvertUtils;

		jwsmtp::mailer mail("", senderMail.c_str(),
			fromLocal<CharType::UTF8>(title).c_str(), "",
			senderServer.c_str(), jwsmtp::mailer::SMTP_PORT, false);
		mail.setmessageHTML(fromLocal<CharType::UTF8>(content).c_str());

		for (const auto &receiverMail : receiverMails)
		{
			mail.addrecipient(receiverMail);
		}

		return __sendMail(senderMail, senderPassword, mail);
	}

	std::pair<bool, std::string> __sendMail(const std::string & senderMail, const std::string & senderPassword, jwsmtp::mailer & mail)
	{
		static const std::string SendMailOKCode("250");

		mail.authtype(jwsmtp::mailer::authtype::PLAIN);

		mail.username(senderMail);
		mail.password(senderPassword);

		mail.send();
		return std::make_pair(mail.response().find(SendMailOKCode) == 0, mail.response());
	}
};
