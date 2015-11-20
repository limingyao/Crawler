package com.limyao.crawler;

import java.io.IOException;
import java.io.PrintWriter;
import java.io.StringWriter;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.HashMap;
import java.util.List;
import java.util.Map;

import javax.naming.ConfigurationException;

import org.apache.log4j.Logger;
import org.jsoup.Jsoup;
import org.jsoup.nodes.Document;
import org.jsoup.nodes.Element;
import org.jsoup.select.Elements;

import com.limyao.util.Configuration;
import com.limyao.util.JCNHtmlUnit;

public class Crawler {
	
	private static final Logger log = Logger.getLogger(Crawler.class.getName());
	
	private Configuration configuration;
	private String surl = "";

	public Crawler() {
		try {
			configuration = new Configuration(System.getProperty("user.dir") + "\\config\\config.xml");
		} catch (ConfigurationException e) {
			StringWriter sw = new StringWriter();
			e.printStackTrace(new PrintWriter(sw));
			log.info(sw.toString());
		}
		surl = configuration.getValue("url");
	}
	
	public String[] getWeather() {
		List<String[]> list = getTomorrowInfo();
		while (list == null) {
			list = getTomorrowInfo();
		}
		StringBuffer sb = new StringBuffer();
		String title = new String();
		for (String[] strs : list) {
			if (strs.length == 3) {
				sb.append(strs[0] + " " + strs[1] + " " + strs[2] + "\n");
			} else {
				title = strs[0];
			}
		}
		String[] res = { title, sb.toString() };
		return res;
	}
	
	private List<String[]> getTomorrowInfo() {
		List<String[]> list = new ArrayList<String[]>();
		
		String tomorrowurl = surl;
		log.info("闵行明天url " + tomorrowurl);

		Map<String, String> map = new HashMap<String, String>();
		map.put("hourChart", "");

		String htmlString = JCNHtmlUnit.getHtmlString(tomorrowurl, "D:\\log.txt", map, 30 * 1000);

		Document doc = Jsoup.parse(htmlString);

		Elements divs = doc.getElementsByAttributeValue("class", "module module-hour-trend");
		Elements bdivs = Jsoup.parse(divs.html()).getElementsByAttributeValue("class", "btitle");
		String[] title = {bdivs.text()};
		list.add(title);

		divs = doc.getElementsByAttributeValue("class", "ehour_data_list");
		Elements lis = Jsoup.parse(divs.html()).getElementsByTag("li");
		divs = doc.getElementsByAttributeValue("id", "hourChart");
		divs = Jsoup.parse(divs.html()).getElementsByTag("div");
		StringBuffer sb = new StringBuffer();
		for(Element element:lis){
			String time = Jsoup.parse(element.html()).getElementsByAttributeValue("class", "time").text();
			String state = Jsoup.parse(element.html()).getElementsByAttributeValue("class", "f_wz").text();
			if(sb.length()==0){
				sb.append(time+","+state);
			}else{
				sb.append(";"+time+","+state);
			}
		}
		StringBuffer sbt = new StringBuffer();
		for(Element element:divs){
			if(element.html().contains("℃")){
				if(sbt.length()==0){
					sbt.append(element.html());
				}else{
					sbt.append(";"+element.html());
				}
			}
		}
		String[] strs1 = sb.toString().split(";");
		String[] strs2 = sbt.toString().split(";");

		for (int i = 0; i < strs1.length && strs1.length == strs2.length; i++) {
			String[] strs3 = strs1[i].split(",");
			if (strs3.length < 2) {
				return null;
			}
			String time = strs3[0];
			String state = strs3[1];
			String temperature = strs2[i];
			String[] str = { time, state, temperature };
			list.add(str);
		}
		return list;
	}
}
