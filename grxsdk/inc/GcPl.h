﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef GCPL_H
#define GCPL_H

#if _MSC_VER >= 1000
#  pragma once
#endif

#ifdef _GCPL_BUILD
#  define GCPL_PORT __declspec(dllexport)
#else
#  define GCPL_PORT
#endif

#endif