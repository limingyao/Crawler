package com.limyao.tags;

import org.htmlparser.tags.CompositeTag;

public class PreTag extends CompositeTag {

	private static final long serialVersionUID = 6447713506714669497L;

	private static final String[] mIds = new String[] { "PRE" };

	private static final String[] mEndTagEnders = new String[] { "P", "BODY", "HTML" };

	public PreTag() {
	}

	public String[] getIds() {
		return (mIds);
	}

	public String[] getEndTagEnders() {
		return (mEndTagEnders);
	}
}
