#include "MailTest.h"
#include "StringConvertUtils.h"
#include <jwsmtp/jwsmtp.h>

void MailTest(void)
{
	jwsmtp::mailer m("373611500@qq.com"/*接收者*/, "fuookami@163.com"/*发送者*/, StringConvertUtils::fromLocal<StringConvertUtils::StringCodeId::UTF8>("这是一个标题").c_str(),
		StringConvertUtils::fromLocal<StringConvertUtils::StringCodeId::UTF8>("这是一个邮件内容").c_str(),
		"smtp.163.com", jwsmtp::mailer::SMTP_PORT, false);

	//经过测试，163支持的auth认证是PLAIN模式  
	m.authtype(jwsmtp::mailer::PLAIN);

	//这里输入认证用户名，注意哦，需要是***@163.com的用户名  
	m.username("fuookami@163.com");
	//这里输入密码  
	m.password("a373611500a");

	m.send(); // 这里发送邮件，需要注意的是，这里是同步模式哦！  
	std::cout << m.response() << std::endl;//这里返回是否成功，250代表发送邮件成功;  
}
