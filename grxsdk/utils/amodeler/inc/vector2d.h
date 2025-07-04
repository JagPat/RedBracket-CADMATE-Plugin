﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef GMODELER_INC_VECTOR2D_H
#define GMODELER_INC_VECTOR2D_H

#include <math.h>
#include "global.h"

GMODELER_NAMESPACE_BEGIN

class DllImpExp Vector2d
{
public:
  Vector2d()
  {
  }
  Vector2d(double xx, double yy) : x(xx), y(yy)
  {
  }

  Vector2d operator+(Vector2d v) const
  {
    return Vector2d(x + v.x, y + v.y);
  }
  Vector2d operator-(Vector2d v) const
  {
    return Vector2d(x - v.x, y - v.y);
  }
  double operator%(Vector2d v) const
  {
    return x * v.x + y * v.y;
  }
  Vector2d operator-() const
  {
    return Vector2d(-x, -y);
  }

  Vector2d operator*(double scalar) const
  {
    return Vector2d(x * scalar, y * scalar);
  }
  Vector2d operator/(double scalar) const
  {
    return Vector2d(x / scalar, y / scalar);
  }

  double operator[](int index) const
  {
    return (&x)[index];
  }
  double& operator[](int index)
  {
    return (&x)[index];
  }

  double length() const
  {
    return sqrt(x * x + y * y);
  }
  double lengthSqrd() const
  {
    return x * x + y * y;
  }
  double lengthRect() const
  {
    return fabs(x) + fabs(y);
  }

  Vector2d normalize() const;

  double dist(Vector2d v) const
  {
    return (*this - v).length();
  }
  double distSqrd(Vector2d v) const
  {
    return (*this - v).lengthSqrd();
  }
  double distRect(Vector2d v) const
  {
    return (*this - v).lengthRect();
  }

  bool isEqual(Vector2d v, double epsSqrd = epsNorSqrd()) const
  {
    return (distSqrd(v) <= epsSqrd);
  }
  bool isEqualRect(Vector2d v, double epsRect = epsNor()) const
  {
    return (distRect(v) <= epsRect);
  }
  bool isExactNull() const
  {
    return x == 0 && y == 0;
  }
  bool isParallel(const Vector2d&) const;
  bool isPerpend(const Vector2d&) const;

  void operator+=(Vector2d v)
  {
    x += v.x;
    y += v.y;
  }
  void operator-=(Vector2d v)
  {
    x -= v.x;
    y -= v.y;
  }
  void operator*=(double scalar)
  {
    x *= scalar;
    y *= scalar;
  }
  void operator/=(double scalar)
  {
    x /= scalar;
    y /= scalar;
  }

  Vector2d perpend() const
  {
    return Vector2d(-y, x);
  }

  double angle(const Vector2d&) const;
  bool isNull(double epsSqrd = epsNorSqrd()) const
  {
    return lengthSqrd() <= epsSqrd;
  }

  operator GcGeVector2d&()
  {
    return *((GcGeVector2d*)this);
  }
  operator const GcGeVector2d&() const
  {
    return *((const GcGeVector2d*)this);
  }

  static Vector2d& cast(GcGeVector2d& v)
  {
    return *(Vector2d*)&v;
  }

  Vector2d(const GcGeVector2d& v)
  {
    *this = *(Vector2d*)&v;
  }

  double x, y;

  static const Vector2d kNull;
  static const Vector2d kXDir;
  static const Vector2d kYDir;
};

inline Vector2d Vector2d::normalize() const
{
  const double l = length();
  return (l > kEpsZero ? Vector2d(x / l, y / l) : Vector2d(0, 0));
}

inline double det(Vector2d v1, Vector2d v2)
{
  return (v1.x * v2.y - v1.y * v2.x);
}

GMODELER_NAMESPACE_END
#endif
