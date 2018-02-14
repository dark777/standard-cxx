#include <stdio.h>
#include <string.h>

int main ()
{
  char email [ 50 ], dominio [ 10 ];
  
  printf ( "\nDigite seu e-mail: " );
  scanf ( "%s", email );

  char google [ 11 ];

  int i, j;

  for( i = strlen(email)-10, j = 0; j < 10; j++, i++ )google[j] = email[i];

  if( strcmp(google, "@gmail.com" ) == 0 )
   printf ( "Email valido.\n" );
  else
   printf ( "Email invalido.\n" );
  return 0;
} 
