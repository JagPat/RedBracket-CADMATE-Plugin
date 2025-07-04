﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef GMODELER_INC_IPOINT3D_H
#define GMODELER_INC_IPOINT3D_H

#include "ipoint2d.h"
#include "ivect3d.h"

GMODELER_NAMESPACE_BEGIN

class DllImpExp IntPoint3d
{
public:
  IntPoint3d()
  {
  }
  IntPoint3d(int xx, int yy, int zz) : x(xx), y(yy), z(zz)
  {
  }

  IntPoint3d operator+(IntPoint3d p) const
  {
    return IntPoint3d(x + p.x, y + p.y, z + p.z);
  }
  IntPoint3d operator+(IntVector3d v) const
  {
    return IntPoint3d(x + v.x, y + v.y, z + v.z);
  }
  IntVector3d operator-(IntPoint3d p) const
  {
    return IntVector3d(x - p.x, y - p.y, z - p.z);
  }
  IntPoint3d operator-(IntVector3d v) const
  {
    return IntPoint3d(x - v.x, y - v.y, z - v.z);
  }
  IntPoint3d operator-() const
  {
    return IntPoint3d(-x, -y, -z);
  }

  int operator[](int index) const
  {
    return (&x)[index];
  }
  int& operator[](int index)
  {
    return (&x)[index];
  }

  bool operator==(IntPoint3d p) const
  {
    return x == p.x && y == p.y && z == p.z;
  }
  bool operator!=(IntPoint3d p) const
  {
    return !(*this == p);
  }

  void operator+=(IntPoint3d p)
  {
    x += p.x;
    y += p.y;
    z += p.z;
  }
  void operator+=(IntVector3d v)
  {
    x += v.x;
    y += v.y;
    z += v.z;
  }
  void operator-=(IntPoint3d p)
  {
    x -= p.x;
    y -= p.y;
    z -= p.z;
  }
  void operator-=(IntVector3d v)
  {
    x -= v.x;
    y -= v.y;
    z -= v.z;
  }

  const IntPoint2d& toIntPoint2d() const
  {
    return *((IntPoint2d*)this);
  }

  int x, y, z;

  static const IntPoint3d kNull;
};

GMODELER_NAMESPACE_END
#endif
