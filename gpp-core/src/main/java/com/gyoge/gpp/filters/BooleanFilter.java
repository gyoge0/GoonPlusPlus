package com.gyoge.gpp.filters;

/**
 * BooleanFilter
 * <p>
 * Only allows the use of Booleans in a Document.
 *
 * @see <a href="https://stackoverflow.com/a/11093360">StackOverflow</a>
 */
public class BooleanFilter extends Filter {

    @Override
    protected boolean test(String text) {
        try {
            //noinspection ResultOfMethodCallIgnored
            Boolean.parseBoolean(text);
            return true;
        } catch (NumberFormatException e) {
            return false;
        }
    }

}