package com.gyoge.gpp.nowrap;

import javax.swing.text.*;

/**
 * @author Stanislav Lapitsky
 * @version 1.0
 * @see <a href=http://java-sl.com/src/wrap_src.html>source</a>
 */
public class WrapLabelView extends LabelView {

    public WrapLabelView(Element elem) {
        super(elem);
    }

    @Override
    public int getBreakWeight(int axis, float pos, float len) {
        if (axis == View.X_AXIS) {
            checkPainter();
            int p0 = getStartOffset();
            int p1 = getGlyphPainter().getBoundedPosition(this, p0, pos, len);
            if (p1 == p0) {
                // can't even fit a single character
                return View.BadBreakWeight;
            }
            try {
                //if the view contains line break char return forced break
                if (getDocument().getText(p0, p1 - p0).contains("\r")) {
                    return View.ForcedBreakWeight;
                }
            } catch (BadLocationException ex) {
                //should never happen
            }
        }
        return super.getBreakWeight(axis, pos, len);
    }

    @Override
    public View breakView(int axis, int p0, float pos, float len) {
        if (axis == View.X_AXIS) {
            checkPainter();
            int p1 = getGlyphPainter().getBoundedPosition(this, p0, pos, len);
            try {
                //if the view contains line break char break the view
                int index = getDocument().getText(p0, p1 - p0).indexOf("\r");
                if (index >= 0) {
                    return createFragment(p0, p0 + index + 1);
                }
            } catch (BadLocationException ex) {
                //should never happen
            }
        }
        return super.breakView(axis, p0, pos, len);
    }
}
