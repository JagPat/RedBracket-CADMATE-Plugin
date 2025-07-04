﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef _GNavFilterArray_h
#define _GNavFilterArray_h

#if _MSC_VER >= 1000
#  pragma once
#endif

#pragma warning(push)
#pragma warning(disable : 4275)
class GCAD_PORT CNavFilterArray : public CNavArray<CNavFilter>
{
public:
  CNavFilterArray();
  ~CNavFilterArray();

public:
  void SetFrom(LPCTSTR str, BOOL bUseFullDescrition = FALSE);
  int Find(LPCTSTR str);
  int FindExact(LPCTSTR str);
};
#pragma warning(pop)

#endif
