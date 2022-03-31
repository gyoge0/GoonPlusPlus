package com.gyoge.gpp.filters;

/**
 * AnyFilter
 * <p>
 * A filter that allows any value to pass through.
 *
 * @see <a href="https://stackoverflow.com/a/11093360">StackOverflow</a>
 */
public class AnyFilter extends Filter {

    @Override
    protected boolean test(String text) {
        return true;
    }

}