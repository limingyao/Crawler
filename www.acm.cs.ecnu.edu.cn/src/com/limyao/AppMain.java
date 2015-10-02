package com.limyao;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.ArrayList;
import java.util.List;

import org.apache.http.Consts;
import org.apache.http.NameValuePair;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.entity.UrlEncodedFormEntity;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.cookie.Cookie;
import org.apache.http.impl.client.BasicResponseHandler;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.impl.conn.PoolingClientConnectionManager;
import org.apache.http.message.BasicNameValuePair;

public class AppMain {

	private static DefaultHttpClient httpclient;
	private static HttpPost httpost;
	private static List<NameValuePair> nvps;
	private static String res;

	public static void ShowCookies() {
		System.out.println("Post logon cookies:");
		List<Cookie> cookies = httpclient.getCookieStore().getCookies();
		if (cookies.isEmpty()) {
			System.out.println("None");
		} else {
			for (int i = 0; i < cookies.size(); i++) {
				System.out.println("- " + cookies.get(i).toString());
			}
		}
	}

	public static void LogIn(String userName, String userPasswd) {
		httpclient = new DefaultHttpClient(new PoolingClientConnectionManager());
		httpost = new HttpPost("http://202.120.80.191/loginpage.php");
		// 添加参数
		nvps = new ArrayList<NameValuePair>();
		nvps.add(new BasicNameValuePair("LGUser", userName));
		nvps.add(new BasicNameValuePair("LGPsd", userPasswd));
		nvps.add(new BasicNameValuePair("LoginSubmit", "Login"));
		httpost.setEntity(new UrlEncodedFormEntity(nvps, Consts.UTF_8));
		BasicResponseHandler responseHandler = new BasicResponseHandler();
		try {
			System.out.println(httpclient.execute(httpost, responseHandler));
		} catch (ClientProtocolException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public static void Search(String userName) {

		httpost = new HttpPost("http://202.120.80.191/Search.php");
		// 添加参数
		nvps = new ArrayList<NameValuePair>();
		// username=lmy&ProblemID=&Result=AC&Submit=Search
		nvps.add(new BasicNameValuePair("username", userName));
		nvps.add(new BasicNameValuePair("ProblemID", ""));
		nvps.add(new BasicNameValuePair("Result", "AC"));
		nvps.add(new BasicNameValuePair("Submit", "Search"));
		httpost.setEntity(new UrlEncodedFormEntity(nvps, Consts.UTF_8));

		BasicResponseHandler responseHandler = new BasicResponseHandler();
		try {
			res = httpclient.execute(httpost, responseHandler);
		} catch (ClientProtocolException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public static String getPage(String url) {
		HttpGet get = new HttpGet(url);
		String str = "";
		BasicResponseHandler responseHandler = new BasicResponseHandler();
		try {
			str = httpclient.execute(get, responseHandler);
		} catch (ClientProtocolException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
		try {
			return new String(str.getBytes("ISO-8859-1"),"gbk");
		} catch (UnsupportedEncodingException e) {
			e.printStackTrace();
		}
		return "";
	}

	public static void Close() {
		httpclient.getConnectionManager().shutdown();
	}

	public static void main(String[] args) throws IOException {
		String userName = "******";
		String userPasswd = "******";
		LogIn(userName, userPasswd);
		ShowCookies();
		Search(userName);
		DealHTML dh = new DealHTML();
		dh.getUrl(res);
		List<String> keyList = dh.getKey();
		List<String> valueList = dh.getValue();
		for (int i = 0; i < keyList.size(); i++) {
			String no = keyList.get(i);
			String url = "http://202.120.80.191/" + valueList.get(i);
			String ret = dh.getContext(getPage(url));
			WriteTXT wt = new WriteTXT();
			wt.writeWithName(no, ret);
		}
		Close();
	}
}
