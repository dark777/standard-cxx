#ifndef _MAIN_CXX
#define _MAIN_CXX
#include "contact.hxx"


#ifdef _WIN32
auto main() -> void
#else
auto main(void) -> int
#endif

{
 readFile();
 doMenu();
}

#endif
