package com.gyoge.gpp.filters;

/**
 * DoubleFilter
 * <p>
 * Only allows the use of Doubles in a Document.
 *
 * @see <a href="https://stackoverflow.com/a/11093360">StackOverflow</a>
 */
public class DoubleFilter extends Filter {

    @Override
    protected boolean test(String text) {
        try {
            Double.parseDouble(text);
            return true;
        } catch (NumberFormatException e) {
            return false;
        }
    }

}