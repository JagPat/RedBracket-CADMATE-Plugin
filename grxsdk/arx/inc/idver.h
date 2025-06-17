﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#pragma once
#include "../../inc/idver.h"
#include "id.h"

#ifndef GC_BLDENV
#  ifndef GCADV_BLDMAJOR
#    define GCADV_BLDMAJOR 0
#  endif

#  ifndef GCADV_BLDMINOR
#    define GCADV_BLDMINOR 0
#  endif

#  ifndef GCADV_BLDBRANCH
#    define GCADV_BLDBRANCH 0
#  endif

#  ifndef GCADV_BLDSTREAM
#    define GCADV_BLDSTREAM O
#  endif

#else
#  include "_idver.h"
#endif

#ifndef ACADV_BLDMAJOR
#  define ACADV_BLDMAJOR GCADV_BLDMAJOR
#endif

#ifndef ACADV_BLDMINOR
#  define ACADV_BLDMINOR GCADV_BLDMINOR
#endif

#ifndef ACADV_BLDBRANCH
#  define ACADV_BLDBRANCH GCADV_BLDBRANCH
#endif

#ifndef ACADV_BLDSTREAM
#  define ACADV_BLDSTREAM GCADV_BLDSTREAM
#endif

#ifndef ACADV_RCFILEVER1
#  define ACADV_RCFILEVER1 GCADV_RCFILEVER1
#endif

#ifndef ACADV_RCFILEVER2
#  define ACADV_RCFILEVER2 GCADV_RCFILEVER2
#endif

#ifndef ACADV_RCFILEVER3
#  define ACADV_RCFILEVER3 GCADV_RCFILEVER3
#endif

#ifndef ACADV_RCFILEVER4
#  define ACADV_RCFILEVER4 GCADV_RCFILEVER4
#endif

#ifndef ACADV_RCFILEVER5
#  define ACADV_RCFILEVER5 GCADV_RCFILEVER5
#endif

#ifndef ACADV_RCFILEVER1_CORRECTION
#  define ACADV_RCFILEVER1_CORRECTION GCADV_RCFILEVER1_CORRECTION
#endif

#ifndef ACADV_RCFILEVERSTR
#  define ACADV_RCFILEVERSTR GCADV_RCFILEVERSTR
#endif

#ifndef ACADV_PRODVERSION
#  define ACADV_PRODVERSION GCADV_PRODVERSION
#endif

#ifndef ACADV_VERNUM
#  define ACADV_VERNUM GCADV_VERNUM
#endif

#ifndef ACADV_VERNAME
#  define ACADV_VERNAME GCADV_VERNAME
#endif

#ifndef ACADV_VERFULL
#  define ACADV_VERFULL GCADV_VERFULL
#endif

#ifndef ACADV_BLDVERSTR
#  define ACADV_BLDVERSTR GCADV_BLDVERSTR
#endif

#ifndef ACAD_COPYRIGHT
#  define ACAD_COPYRIGHT GCAD_COPYRIGHT
#endif

#ifndef ACAD_COPYRIGHT_YEAR
#  define ACAD_COPYRIGHT_YEAR GCAD_COPYRIGHT_YEAR
#endif

#ifndef ACAD_TLBVERSION_MAJOR
#  define ACAD_TLBVERSION_MAJOR GCAD_TLBVERSION_MAJOR
#endif

#ifndef ACAD_TLBVERSION_MINOR
#  define ACAD_TLBVERSION_MINOR GCAD_TLBVERSION_MINOR
#endif

#ifndef ACAD_TLBVERSION
#  define ACAD_TLBVERSION GCAD_TLBVERSION
#endif

#ifndef ACAD_TLBVERSIONSTR
#  define ACAD_TLBVERSIONSTR GCAD_TLBVERSIONSTR
#endif

#ifndef ACDB_TLBVERSION_MAJOR
#  define ACDB_TLBVERSION_MAJOR GCDB_TLBVERSION_MAJOR
#endif

#ifndef ACDB_TLBVERSION_MINOR
#  define ACDB_TLBVERSION_MINOR GCDB_TLBVERSION_MINOR
#endif

#ifndef ACDB_TLBVERSION
#  define ACDB_TLBVERSION GCDB_TLBVERSION
#endif

#ifndef ACDB_TLBVERSIONSTR
#  define ACDB_TLBVERSIONSTR GCDB_TLBVERSIONSTR
#endif
