package com.limyao.main;

import java.io.PrintWriter;
import java.io.StringWriter;

import org.apache.log4j.Logger;
import org.quartz.Job;
import org.quartz.JobExecutionContext;
import org.quartz.JobExecutionException;

import com.limyao.job.JobThread;

public class Main implements Job {

	private static final Logger log = Logger.getLogger(Main.class.getName());

	public static void main(String[] args) {
		Main m = new Main();
		try {
			m.execute(null);
		} catch (JobExecutionException e) {
			StringWriter sw = new StringWriter();
			e.printStackTrace(new PrintWriter(sw));
			log.info(sw.toString());
		}
	}

	@Override
	public void execute(JobExecutionContext arg) throws JobExecutionException {	
		JobThread job = new JobThread();
		while (job.isSuccess()) {
			job.start();
			int timeOut = 5 * 60 * 1000;
			while (job.isAlive() && timeOut >= 0) {
				try {
					Thread.sleep(50);
					timeOut -= 50;
				} catch (InterruptedException e) {
					StringWriter sw = new StringWriter();
					e.printStackTrace(new PrintWriter(sw));
					log.info(sw.toString());
				}
			}
			if(job.isAlive()){
				job.interrupt();
			}
		}
	}
}
