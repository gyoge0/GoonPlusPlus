package com.gyoge.gpp.nowrap;

import javax.swing.text.*;

/**
 * @author Stanislav Lapitsky
 * @version 1.0
 * @see <a href=http://java-sl.com/src/wrap_src.html>source</a>
 */
public class NoWrapParagraphView extends ParagraphView {

    public NoWrapParagraphView(Element elem) {
        super(elem);
    }

    @Override
    public void layout(int width, int height) {
        super.layout(Short.MAX_VALUE, height);
    }

    @Override
    public float getMinimumSpan(int axis) {
        return super.getPreferredSpan(axis);
    }
}
