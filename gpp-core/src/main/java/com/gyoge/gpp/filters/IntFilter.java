package com.gyoge.gpp.filters;

/**
 * IntFilter
 * <p>
 * Only allows the use of Integers in a Document.
 *
 * @see <a href="https://stackoverflow.com/a/11093360">StackOverflow</a>
 */
public class IntFilter extends Filter {

    @Override
    protected boolean test(String text) {
        try {
            Integer.parseInt(text);
            return true;
        } catch (NumberFormatException e) {
            return false;
        }
    }

}