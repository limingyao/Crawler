package com.limyao.util;

import javax.naming.ConfigurationException;

public class MailUtil {

	private String title;
	private String content;
	private String MailServerHost;
	private String MailServerPort;
	private String email;
	private String passwd;
	private String fromemailaddress;
	private String toemailaddress;
	private Configuration configuration;

	public MailUtil(String title, String content) {
		this.title = title;
		this.content = content;
		init();
	}

	private void init() {
		try {
			configuration = new Configuration(System.getProperty("user.dir") + "\\config\\config.xml");
		} catch (ConfigurationException e) {
			e.printStackTrace();
		}
		email = configuration.getValue("email");
		passwd = configuration.getValue("passwd");
		MailServerHost = configuration.getValue("MailServerHost");
		MailServerPort = configuration.getValue("MailServerPort");
		fromemailaddress = configuration.getValue("fromemailaddress");
		toemailaddress = configuration.getValue("toemailaddress");
	}

	public void send() {
		// 这个类主要是设置邮件
		MailSenderInfo mailInfo = new MailSenderInfo();
		mailInfo.setMailServerHost(MailServerHost);
		mailInfo.setMailServerPort(MailServerPort);
		mailInfo.setValidate(true);

		mailInfo.setUserName(email);
		mailInfo.setPassword(passwd);// 您的邮箱密码

		mailInfo.setFromAddress(fromemailaddress);
		mailInfo.setToAddress(toemailaddress);
		mailInfo.setSubject(title);
		mailInfo.setContent(content);
		// 这个类主要来发送邮件
		SimpleMailSender sms = new SimpleMailSender();
		sms.sendTextMail(mailInfo);// 发送文体格式
		// sms.sendHtmlMail(mailInfo);// 发送html格式
	}
}
