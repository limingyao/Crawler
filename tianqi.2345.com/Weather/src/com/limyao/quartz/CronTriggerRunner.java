package com.limyao.quartz;

import java.io.PrintWriter;
import java.io.StringWriter;
import java.text.ParseException;

import javax.naming.ConfigurationException;

import org.apache.log4j.Logger;
import org.quartz.CronExpression;
import org.quartz.Scheduler;
import org.quartz.SchedulerException;
import org.quartz.SchedulerFactory;
import org.quartz.impl.JobDetailImpl;
import org.quartz.impl.StdSchedulerFactory;
import org.quartz.impl.triggers.CronTriggerImpl;

import com.limyao.main.Main;
import com.limyao.util.Configuration;

public class CronTriggerRunner {
	private static final Logger log = Logger.getLogger(CronTriggerRunner.class.getName());
	private static Configuration configuration;
	private static String cronExpression;

	public static void main(String[] args) {
		try {
			configuration = new Configuration(System.getProperty("user.dir") + "\\config\\config.xml");
			cronExpression = configuration.getValue("cronExpression");
			SchedulerFactory sf = new StdSchedulerFactory();
			Scheduler scheduler = sf.getScheduler();

			JobDetailImpl jobDetail = new JobDetailImpl();
			jobDetail.setJobClass(Main.class);
			jobDetail.setGroup("Group");
			jobDetail.setName("Job");

			CronTriggerImpl cronTrigger = new CronTriggerImpl();
			cronTrigger.setGroup("Croup");
			cronTrigger.setName("CronTrigger");
			CronExpression expression = new CronExpression(cronExpression);
			cronTrigger.setCronExpression(expression);

			scheduler.scheduleJob(jobDetail, cronTrigger);
			scheduler.start();
		} catch (SchedulerException e) {
			StringWriter sw = new StringWriter();
			e.printStackTrace(new PrintWriter(sw));
			log.info(sw.toString());
		} catch (ParseException e) {
			StringWriter sw = new StringWriter();
			e.printStackTrace(new PrintWriter(sw));
			log.info(sw.toString());
		} catch (ConfigurationException e) {
			StringWriter sw = new StringWriter();
			e.printStackTrace(new PrintWriter(sw));
			log.info(sw.toString());
		} catch (Exception e) {
			StringWriter sw = new StringWriter();
			e.printStackTrace(new PrintWriter(sw));
			log.info(sw.toString());
		}
	}
}
