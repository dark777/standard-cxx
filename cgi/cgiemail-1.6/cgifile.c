/**********************************************************************
 *  cgiemail -- send email via cgi
 *
 * Copyright 1997 by the Massachusetts Institute of Technology
 * For copying and distribution information, please see the file
 * <mit-copyright.h>.
 **********************************************************************/
#include "mit-copyright.h"

#include <stdlib.h>
#include "cgi.h"

main() { exit(  cgi_standard_file()  ); }
