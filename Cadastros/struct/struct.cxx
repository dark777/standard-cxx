#include <stdio.h>
#include <stddef.h>
//Struct declaration
//clang++ -o str struct.cxx









int main(void)
{
    struct car { const char *make; const char *model; int year; }; // declares the struct type
    
    // declares and initializes an object of a previously-declared struct type
    car c = {.year=1923, .make="Nash", .model="48 Sports Touring Car"};
    
    printf("car: %d %s %s\n", c.year, c.make, c.model);
    
    // declares a struct type, an object of that type, and a pointer to it
    struct spaceship { const char *make; const char *model; const char *year; }
        ship = {"Incom Corporation", "T-65 X-wing starfighter", "128 ABY"}, *pship = &ship;
    
    printf("spaceship: %s %s %s\n", ship.year, ship.make, ship.model);
    
    // addresses increase in order of definition
    // padding may be inserted
    struct A { char a; double b; char c;};
    printf("offset of char a = %zu\noffset of double b = %zu\noffset of char c = %zu\n"
           "sizeof(struct A)=%zu\n", offsetof(struct A, a), offsetof(struct A, b),
           offsetof(struct A, c), sizeof(struct A));
    
    struct B { char a; char b; double c;};
    printf("offset of char a = %zu\noffset of char b = %zu\noffset of double c = %zu\n"
           "sizeof(struct B)=%zu\n", offsetof(struct B, a), offsetof(struct B, b),
           offsetof(struct B, c), sizeof(struct B));
    
    // A pointer to a struct can be cast to a pointer to its first member and vice versa
    char* pmake = (char*)&ship;
    pship = (struct spaceship *)pmake;
}