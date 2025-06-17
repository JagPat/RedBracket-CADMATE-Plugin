﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef GC_GEMAT2D_H
#define GC_GEMAT2D_H

#include "gegbl.h"
#include "gepnt2d.h"
#pragma pack(push, 8)

class GcGeVector2d;
class GcGeLine2d;
class GcGeTol;

class GcGeMatrix2d
{
public:
  GE_DLLEXPIMPORT GcGeMatrix2d();
  GE_DLLEXPIMPORT GcGeMatrix2d(const GcGeMatrix2d& src);

  GE_DLLDATAEXIMP static const GcGeMatrix2d kIdentity;

  GE_DLLEXPIMPORT GcGeMatrix2d& setToIdentity();

  GE_DLLEXPIMPORT GcGeMatrix2d operator*(const GcGeMatrix2d& mat) const;
  GE_DLLEXPIMPORT GcGeMatrix2d& operator*=(const GcGeMatrix2d& mat);
  GE_DLLEXPIMPORT GcGeMatrix2d& preMultBy(const GcGeMatrix2d& leftSide);
  GE_DLLEXPIMPORT GcGeMatrix2d& postMultBy(const GcGeMatrix2d& rightSide);
  GE_DLLEXPIMPORT GcGeMatrix2d& setToProduct(const GcGeMatrix2d& mat1, const GcGeMatrix2d& mat2);

  GE_DLLEXPIMPORT GcGeMatrix2d& invert();
  GE_DLLEXPIMPORT GcGeMatrix2d inverse() const;

  GE_DLLEXPIMPORT Gsoft::Boolean isSingular(const GcGeTol& tol = GcGeContext::gTol) const;

  GE_DLLEXPIMPORT GcGeMatrix2d& transposeIt();
  GE_DLLEXPIMPORT GcGeMatrix2d transpose() const;

  GE_DLLEXPIMPORT bool operator==(const GcGeMatrix2d& mat) const;
  GE_DLLEXPIMPORT bool operator!=(const GcGeMatrix2d& mat) const;
  GE_DLLEXPIMPORT bool isEqualTo(const GcGeMatrix2d& mat, const GcGeTol& tol = GcGeContext::gTol) const;

  GE_DLLEXPIMPORT Gsoft::Boolean isUniScaledOrtho(const GcGeTol& tol = GcGeContext::gTol) const;
  GE_DLLEXPIMPORT Gsoft::Boolean isScaledOrtho(const GcGeTol& tol = GcGeContext::gTol) const;
  GE_DLLEXPIMPORT double scale(void);
  GE_DLLEXPIMPORT double det() const;

  GE_DLLEXPIMPORT GcGeMatrix2d& setTranslation(const GcGeVector2d& vec);
  GE_DLLEXPIMPORT GcGeVector2d translation() const;

  GE_DLLEXPIMPORT Gsoft::Boolean isConformal(double& scale,
                                             double& angle,
                                             Gsoft::Boolean& isMirror,
                                             GcGeVector2d& reflex) const;

  GE_DLLEXPIMPORT GcGeMatrix2d& setCoordSystem(const GcGePoint2d& origin,
                                               const GcGeVector2d& e0,
                                               const GcGeVector2d& e1);
  GE_DLLEXPIMPORT void getCoordSystem(GcGePoint2d& origin, GcGeVector2d& e0, GcGeVector2d& e1) const;

  GE_DLLEXPIMPORT GcGeMatrix2d& setToTranslation(const GcGeVector2d& vec);
  GE_DLLEXPIMPORT GcGeMatrix2d& setToRotation(double angle, const GcGePoint2d& center = GcGePoint2d::kOrigin);
  GE_DLLEXPIMPORT GcGeMatrix2d& setToScaling(double scaleAll, const GcGePoint2d& center = GcGePoint2d::kOrigin);
  GE_DLLEXPIMPORT GcGeMatrix2d& setToMirroring(const GcGePoint2d& pnt);
  GE_DLLEXPIMPORT GcGeMatrix2d& setToMirroring(const GcGeLine2d& line);
  GE_DLLEXPIMPORT GcGeMatrix2d& setToAlignCoordSys(const GcGePoint2d& fromOrigin,
                                                   const GcGeVector2d& fromE0,
                                                   const GcGeVector2d& fromE1,
                                                   const GcGePoint2d& toOrigin,
                                                   const GcGeVector2d& toE0,
                                                   const GcGeVector2d& toE1);

  GE_DLLEXPIMPORT static GcGeMatrix2d translation(const GcGeVector2d& vec);
  GE_DLLEXPIMPORT static GcGeMatrix2d rotation(double angle, const GcGePoint2d& center = GcGePoint2d::kOrigin);
  GE_DLLEXPIMPORT static GcGeMatrix2d scaling(double scaleAll, const GcGePoint2d& center = GcGePoint2d::kOrigin);
  GE_DLLEXPIMPORT static GcGeMatrix2d mirroring(const GcGePoint2d& pnt);
  GE_DLLEXPIMPORT static GcGeMatrix2d mirroring(const GcGeLine2d& line);
  GE_DLLEXPIMPORT static GcGeMatrix2d alignCoordSys(const GcGePoint2d& fromOrigin,
                                                    const GcGeVector2d& fromE0,
                                                    const GcGeVector2d& fromE1,
                                                    const GcGePoint2d& toOrigin,
                                                    const GcGeVector2d& toE0,
                                                    const GcGeVector2d& toE1);

  GE_DLLEXPIMPORT double operator()(unsigned int, unsigned int) const;
  GE_DLLEXPIMPORT double& operator()(unsigned int, unsigned int);

  double entry[3][3];
};

inline bool GcGeMatrix2d::operator==(const GcGeMatrix2d& otherMatrix) const
{
  return this->isEqualTo(otherMatrix);
}

inline bool GcGeMatrix2d::operator!=(const GcGeMatrix2d& otherMatrix) const
{
  return !this->isEqualTo(otherMatrix);
}

inline double GcGeMatrix2d::operator()(unsigned int row, unsigned int column) const
{
  return entry[row][column];
}

inline double& GcGeMatrix2d::operator()(unsigned int row, unsigned int column)
{
  return entry[row][column];
}

#pragma pack(pop)
#endif
