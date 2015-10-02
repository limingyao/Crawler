package com.limyao.util;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.util.ArrayList;
import java.util.List;

public class Text {

	public static List<String> readText(String filePath) {

		List<String> list = new ArrayList<String>();

		File file = new File(filePath);
		FileInputStream fr = null;
		BufferedReader reader = null;

		try {
			fr = new FileInputStream(file);
			reader = new BufferedReader(new InputStreamReader(fr, "UTF-8"));
			String tempString = null;
			while ((tempString = reader.readLine()) != null) {
				list.add(tempString);
			}
			reader.close();
		} catch (IOException e) {
			e.printStackTrace();
		} finally {
			if (reader != null) {
				try {
					reader.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
			}
			if (fr != null) {
				try {
					fr.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
			}
		}
		return list;
	}

	public static void writeText(String filePath, String content, boolean isAppend) {
		File file = new File(filePath);
		FileOutputStream fr = null;
		BufferedWriter writer = null;
		try {
			fr = new FileOutputStream(file, isAppend);
			writer = new BufferedWriter(new OutputStreamWriter(fr, "UTF-8"));
			writer.write(content);
			writer.newLine();
			writer.close();
		} catch (Exception ex) {
			ex.printStackTrace();
		} finally {
			if (writer != null) {
				try {
					writer.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
			}
			if (fr != null) {
				try {
					fr.close();
				} catch (IOException e) {
					e.printStackTrace();
				}
			}
		}
	}
}
