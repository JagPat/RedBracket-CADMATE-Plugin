﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef GMODELER_INC_DARRAY_H
#define GMODELER_INC_DARRAY_H

#include <stddef.h>
#include "global.h"

GMODELER_NAMESPACE_BEGIN

class DllImpExp Darray
{
public:
  Darray();
  Darray(const Darray& da);
  Darray(int len);
  Darray(void* ptr);
  ~Darray();

  Darray& operator=(const Darray& da);

  void*& operator[](int index);
  void* operator[](int index) const;

  int add(void* ptr);
  void add(int index, void* ptr);
  void add(const Darray& da);
  void del(int index);
  int del(void* ptr);
  int merge(void* ptr);
  int merge(const Darray& da);
  int find(const void* ptr, int startIndex = 0) const;
  int contains(const void* ptr) const;
  void swap(int index1, int index2);
  void intersectWith(const Darray&);

  int length();
  int length() const;

  void resize(int len);
  void init();
  void init(int len);

  void fixAfterMemcopy(void* newAddress);

private:
  enum
  {
    kMinLength = 4
  };

  void** mArray;
  int mAllocLength;
  int mUsedLength;
  void* mFixedArray[kMinLength];

  void extendArray(int len);
  int findLength() const;
};

#pragma warning(push)
#pragma warning(disable : 4068)
#pragma C - Cover off

inline Darray::Darray() : mAllocLength(0), mUsedLength(0)
{
}

inline Darray::Darray(int len) : mAllocLength(0), mUsedLength(0)
{
  resize(len);
}

inline Darray::Darray(void* ptr) : mAllocLength(0), mUsedLength(0)
{
  resize(1);
  mArray[0] = ptr;
  mUsedLength = 1;
}

inline int Darray::contains(const void* ptr) const
{
  return (find(ptr) != -1);
}

inline int Darray::length() const
{
  if ((mUsedLength == 0) || (mArray[mUsedLength - 1] != NULL))
  {
    return mUsedLength;
  }
  else
  {
    return findLength();
  }
}

inline int Darray::length()
{
  if ((mUsedLength != 0) && (mArray[mUsedLength - 1] == NULL))
  {
    mUsedLength = findLength();
  }
  return mUsedLength;
}

inline void Darray::init()
{
  resize(0);
}

inline Darray::~Darray()
{
  init();
}

inline void*& Darray::operator[](int index)
{
  MASSERT(index >= 0);

  if (index >= mUsedLength)
  {
    mUsedLength = index + 1;

    if (index >= mAllocLength)
      extendArray(index + 1);
  }
  return mArray[index];
}

inline void* Darray::operator[](int index) const
{
  MASSERT(index >= 0);

  if (index >= mUsedLength)
  {
    return NULL;
  }
  else
  {
    return mArray[index];
  }
}

#pragma C - Cover on
#pragma warning(pop)

GMODELER_NAMESPACE_END
#endif
