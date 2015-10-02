package com.limyao;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;

public class ReadTXT {
	public String read() {
		StringBuffer sb = new StringBuffer();
		String s;
		try {
			File f = new File("E:\\EOJ\\in.txt");
			if (!f.exists()) {
				f.createNewFile();
			}
			BufferedReader input = new BufferedReader(new FileReader(f));
			while ((s = input.readLine()) != null) {
				sb.append(s + "\n");
			}
			input.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
		return sb.toString();
	}
}
