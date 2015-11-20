package com.limyao.util;

import java.io.PrintWriter;
import java.io.StringWriter;
import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.HashMap;
import java.util.Map;

import org.apache.log4j.Logger;

public class JCNHtmlUnit {
	
	private static final Logger log = Logger.getLogger(JCNHtmlUnit.class.getName());
	private static StringWriter sw = new StringWriter();
	
	public static void main(String[] argv) {
		/*//String str = JCNHtmlUnit.show("http://tianqi.2345.com/tomorrow-58362.htm","D:\\out.txt");
		//System.out.println(str);
		JCNHtmlUnit jcn = new JCNHtmlUnit();*/
		Map<String,String> map = new HashMap<String,String>();
		map.put("hourChart", "");
		JCNHtmlUnit.show("http://tianqi.2345.com/tomorrow-58362.htm", "D:\\out.txt", map, 10000);
	}
	
	
	static {
		System.loadLibrary("JCNHtmlUnit");
	}
	private native static String show(String url, String filename, Map<String, String> map, int timeOut);
	private native static String show(String url, String filename, int timeOut);
	private native static String show(String url, String filename);
	public static String getHtmlString(String url,String filename,Map<String, String> map, int timeOut){
		return show(url,filename,map,timeOut);
	}
	public static String getHtmlString(String url,String filename,int timeOut){
		return show(url,filename,timeOut);
	}
	public static String getHtmlString(String url,String filename){
		return show(url,filename);
	}
	
	public static String GetCurrDateTime() {
		SimpleDateFormat df = new SimpleDateFormat("yyyy-MM-dd HH:mm:ss");
		String currDateTime = df.format(new Date());
		return currDateTime;
	}
	
	public static void printException(Exception e){
		e.printStackTrace(new PrintWriter(sw));
		log.info(sw.toString());
	}
}
