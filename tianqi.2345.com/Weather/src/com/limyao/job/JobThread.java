package com.limyao.job;

import java.io.PrintWriter;
import java.io.StringWriter;

import org.apache.log4j.Logger;

import com.limyao.crawler.Crawler;
import com.limyao.util.JCNHtmlUnit;
import com.limyao.util.MailUtil;

public class JobThread extends Thread {
	
	private static final Logger log = Logger.getLogger(JobThread.class.getName());
	private boolean success = true;

	public boolean isSuccess() {
		return success;
	}

	public void run() {
		try {
			Crawler crawler = new Crawler();
			log.info("开始抓取信息......" + JCNHtmlUnit.GetCurrDateTime());
			String[] res = crawler.getWeather();
			log.info("抓取信息结束......" + JCNHtmlUnit.GetCurrDateTime());
			if (res.length == 2) {
				String title = res[0];
				String content = res[1];
				title = title.substring(0, title.indexOf("小时数据")).replaceAll(" ", "");
				log.info(title);
				log.info(content);
				log.info("开始发送邮件......" + JCNHtmlUnit.GetCurrDateTime());
				MailUtil mail = new MailUtil(title, content);
				mail.send();
				log.info("发送邮件结束......" + JCNHtmlUnit.GetCurrDateTime());
				success = false;
			}
		} catch (Exception e) {
			StringWriter sw = new StringWriter();
			e.printStackTrace(new PrintWriter(sw));
			log.info(sw.toString());
		}
	}
}
