#include <iostream>

struct menu
{
  menu(int, const char*[]);

  menu(int, int, const char*[]);

  menu(int, std::string*);

  menu(int, int, std::string*);
};
