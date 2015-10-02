package com.limyao;

import java.io.BufferedWriter;
import java.io.File;
import java.io.FileWriter;

public class WriteTXT {

	public void write(String s) {
		try {
			File f = new File("E:\\EOJ\\out.txt");
			if (!f.exists()) {
				f.createNewFile();
			}
			BufferedWriter output = new BufferedWriter(new FileWriter(f));
			output.write(s);
			output.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

	public void writeWithName(String name, String str) {
		try {
			File f = new File("E:\\EOJ\\" + name + ".cpp");
			int i = 1;
			while (f.exists()) {
				f = new File("E:\\EOJ\\" + name + "_" + i + ".cpp");
				i++;
			}
			f.createNewFile();
			BufferedWriter output = new BufferedWriter(new FileWriter(f));
			output.write(str);
			output.close();
		} catch (Exception e) {
			e.printStackTrace();
		}
	}
}
