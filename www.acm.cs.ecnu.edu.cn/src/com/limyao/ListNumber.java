package com.limyao;

import java.io.IOException;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

import org.htmlparser.Parser;
import org.htmlparser.PrototypicalNodeFactory;
import org.htmlparser.filters.HasAttributeFilter;
import org.htmlparser.lexer.Lexer;
import org.htmlparser.util.NodeList;
import org.htmlparser.util.ParserException;

import com.limyao.tags.SpanTag;

public class ListNumber {

	public static void dealOnePage(String url, int starNo) {
		try {
			Parser parser = new Parser((HttpURLConnection) (new URL(url)).openConnection());
			NodeList tableSet = parser.extractAllNodesThatMatch(new HasAttributeFilter("bgcolor", "#DDE1FF"));
			parser = new Parser(new Lexer(tableSet.toHtml()));
			NodeList tdSet = parser.extractAllNodesThatMatch(new HasAttributeFilter("tr"));
			parser = new Parser(new Lexer(tdSet.toHtml()));

			PrototypicalNodeFactory p = new PrototypicalNodeFactory();
			p.registerTag(new SpanTag());
			parser.setNodeFactory(p);

			NodeList spanSet = parser.extractAllNodesThatMatch(new HasAttributeFilter("span"));
			int index = 0;
			for (int i = 5; i < spanSet.size(); i = i + 5) {
				String str = spanSet.elementAt(i).toPlainTextString();
				String now = "" + (starNo * 100 + index);
				index++;
				while (str.compareTo(now) != 0) {
					System.out.println(now);
					now = "" + (starNo * 100 + index);
					index++;
				}
				// System.out.println(str);
			}
		} catch (ParserException e) {
			e.printStackTrace();
		} catch (MalformedURLException e) {
			e.printStackTrace();
		} catch (IOException e) {
			e.printStackTrace();
		}
	}

	public static void main(String[] args) {
		int start = 10;
		for (int i = 1; i <= 20; i++) {
			dealOnePage("http://www.acm.cs.ecnu.edu.cn/problemset.php?classify=0&set=" + i, start++);
		}
	}
}
