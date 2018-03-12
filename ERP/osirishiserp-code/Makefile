
EXTRA_DIST =  rules.make configure Makefile.include ../../../../../usr/lib/mono/gac/gdk-sharp/2.12.0.0__35e10195dab3c99f/gdk-sharp.dll ../../../../../usr/lib/mono/gac/glade-sharp/2.12.0.0__35e10195dab3c99f/glade-sharp.dll ../../../../../usr/lib/mono/gac/glib-sharp/2.12.0.0__35e10195dab3c99f/glib-sharp.dll ../../../../../usr/lib/mono/gac/pango-sharp/2.12.0.0__35e10195dab3c99f/pango-sharp.dll ../../../../../usr/lib/mono/gac/Npgsql/2.0.0.0__5d8b90d52f46fda7/Npgsql.dll dll/SX.dll dll/AODL.dll dll/ICSharpCode.SharpZipLib.dll

all: all-recursive

top_srcdir=.
include $(top_srcdir)/config.make
include $(top_srcdir)/Makefile.include
include $(top_srcdir)/rules.make

#include $(top_srcdir)/custom-hooks.make

#Warning: This is an automatically generated file, do not edit!
ifeq ($(CONFIG),DEBUG)
 SUBDIRS =  osiris
endif
ifeq ($(CONFIG),RELEASE)
 SUBDIRS =  osiris
endif


CONFIG_MAKE=$(top_srcdir)/config.make

%-recursive: $(CONFIG_MAKE)
	@set . $$MAKEFLAGS; final_exit=:; \
	case $$2 in --unix) shift ;; esac; \
	case $$2 in *=*) dk="exit 1" ;; *k*) dk=: ;; *) dk="exit 1" ;; esac; \
	make pre-$*-hook prefix=$(prefix) ; \
	for dir in $(call quote_each,$(SUBDIRS)); do \
		case "$$dir" in \
		.) make $*-local || { final_exit="exit 1"; $$dk; };;\
		*) (cd "$$dir" && make $*) || { final_exit="exit 1"; $$dk; };;\
		esac \
	done; \
	make post-$*-hook prefix=$(prefix) ; \
	$$final_exit

$(CONFIG_MAKE):
	echo "You must run configure first"
	exit 1

clean: clean-recursive
install: install-recursive
uninstall: uninstall-recursive

dist: $(CONFIG_MAKE)
	rm -rf $(PACKAGE)-$(VERSION)
	mkdir $(PACKAGE)-$(VERSION)
	make pre-dist-hook distdir=$$distdir
	for dir in $(call quote_each,$(SUBDIRS)); do \
		pkgdir=`pwd`/$(PACKAGE)-$(VERSION); \
		mkdir "$$pkgdir/$$dir" || true; \
		case $$dir in \
		.) make dist-local "distdir=$$pkgdir" || exit 1;; \
		*) (cd "$$dir"; make dist-local "distdir=$$pkgdir/$$dir") || exit 1;; \
		esac \
	done
	(make dist-local distdir=$(PACKAGE)-$(VERSION))
	make post-dist-hook "distsir=$$distdir"
	tar czvf $(PACKAGE)-$(VERSION).tar.gz $(PACKAGE)-$(VERSION)
	rm -rf $(PACKAGE)-$(VERSION)
	@echo "=========================================="
	@echo "$(PACKAGE)-$(VERSION) has been packaged > $(PACKAGE)-$(VERSION).tar.gz"
	@echo "=========================================="

distcheck: dist
	(mkdir test; cd test;  \
	tar xzvf ../$(PACKAGE)-$(VERSION).tar.gz; cd $(PACKAGE)-$(VERSION); \
	./configure --prefix=$$(cd `pwd`/..; pwd); \
	make && make install && make dist);
	rm -rf test
