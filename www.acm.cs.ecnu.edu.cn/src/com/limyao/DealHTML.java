package com.limyao;

import java.util.ArrayList;
import java.util.List;

import org.htmlparser.Node;
import org.htmlparser.Parser;
import org.htmlparser.PrototypicalNodeFactory;
import org.htmlparser.filters.HasAttributeFilter;
import org.htmlparser.lexer.Lexer;
import org.htmlparser.tags.LinkTag;
import org.htmlparser.tags.ParagraphTag;
import org.htmlparser.util.NodeList;
import org.htmlparser.util.ParserException;

import com.limyao.tags.PreTag;

public class DealHTML {
	private List<String> key;
	private List<String> value;

	public List<String> getKey() {
		return key;
	}

	public List<String> getValue() {
		return value;
	}

	public void getUrl(String res) {
		key = new ArrayList<String>();
		value = new ArrayList<String>();
		Parser parser = new Parser(new Lexer(res));
		try {
			NodeList tableSet = parser.extractAllNodesThatMatch(new HasAttributeFilter("style", "border-collapse: collapse"));
			String table = tableSet.toHtml();
			parser = new Parser(new Lexer(table));
			NodeList tdSet = parser.extractAllNodesThatMatch(new HasAttributeFilter("class", "JudgeNormalStyle"));
			Node node[] = tdSet.toNodeArray();
			for (int i = 6; i < node.length; i = i + 8) {
				long runid = Long.parseLong(node[i - 6].toPlainTextString());
				if(runid>268944){
					continue;
				}
				key.add(node[i - 4].toPlainTextString());
				if (node[i].getLastChild() instanceof LinkTag) {
					LinkTag href = (LinkTag) node[i].getLastChild();
					// System.out.println(node[i-4].toPlainTextString()+" "+href.getAttribute("href"));
					value.add(href.getAttribute("href"));
				}
			}
		} catch (ParserException e) {
			e.printStackTrace();
		}
	}

	public String getContext(String res) {
		// System.out.println(res);
		String str = "";
		Parser parser = new Parser(new Lexer(res));
		try {
			NodeList tableSet = parser.extractAllNodesThatMatch(new HasAttributeFilter("body"));

			parser = new Parser(new Lexer(tableSet.toHtml()));
			// 添加自定义标签pre
			PrototypicalNodeFactory p = new PrototypicalNodeFactory();
			p.registerTag(new PreTag());
			p.unregisterTag(new ParagraphTag());
			parser.setNodeFactory(p);

			NodeList list = parser.extractAllNodesThatMatch(new HasAttributeFilter("pre"));
			String line[] = list.toHtml().split("\n");
			for (String s : line) {
				s = s.replaceAll("<[^>]*>", "");
				s = s.replace("Source Code", "");
				s = s.replace("&lt;", "<");
				s = s.replace("&gt;", ">");
				s = s.replace("&nbsp;", "\n");
				s = s.replace("&#91;", "[");
				s = s.replace("&#93;", "]");
				s = s.replace("&#123;", "{");
				s = s.replace("&#125;", "}");
				s = s.replace("&#40;", "(");
				s = s.replace("&#41;", ")");
				s = s.replace("&amp;", "&");
				s = s.replace("&quot;", "\"");
				if (s.contains("Parsed")) {
					int index = s.indexOf("Parsed");
					String time = s.substring(index);
					s = s.substring(0, 1);
					s += "//" + time;
				}
				// System.out.println(s);
				str += s + "\n";
			}
		} catch (ParserException e) {
			e.printStackTrace();
		}
		return str;
	}
}
