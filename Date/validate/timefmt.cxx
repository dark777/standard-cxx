#include <iterator>
#include <iostream>



class timefmt
{
public:
    timefmt(std::string fmt)
        : format(fmt) { }

    friend std::ostream& operator <<(std::ostream &, timefmt const &);

private:
    std::string format;
};

std::ostream& operator <<(std::ostream& os, timefmt const& mt)
{
    std::ostream::sentry s(os);

    if (s)
    {
        std::time_t t = std::time(0);
        std::tm const* tm = std::localtime(&t);
        std::ostreambuf_iterator<char> out(os);

        std::use_facet<std::time_put<char>>(os.getloc())
            .put(out, os, os.fill(),
                 tm, &mt.format[0], &mt.format[0] + mt.format.size());
    }

    os.width(0);

    return os;
}




int main()
{
    std::cout << timefmt("%c");
} 
