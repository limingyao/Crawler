package com.limyao.tags;

import org.htmlparser.tags.CompositeTag;

public class SpanTag extends CompositeTag {

	private static final long serialVersionUID = 1717228961920613821L;

	private static final String[] mIds = new String[] { "span" };

	private static final String[] mEndTagEnders = new String[] { "BODY", "HTML" };

	public SpanTag() {
	}

	public String[] getIds() {
		return (mIds);
	}

	public String[] getEndTagEnders() {
		return (mEndTagEnders);
	}
}
