﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#pragma once

#include "dbmain.h"
#include "dbintar.h"

#pragma pack(push, 8)

class GCDB_PORT GSOFT_NO_VTABLE GcDbProxyObject : public GcDbObject
{
public:
  GCRX_DECLARE_MEMBERS(GcDbProxyObject);

  ~GcDbProxyObject(){};

  virtual int proxyFlags() const = 0;
  virtual const GCHAR* originalClassName() const = 0;
  virtual const GCHAR* originalDxfName() const = 0;
  virtual const GCHAR* applicationDescription() const = 0;
  virtual Gcad::ErrorStatus getReferences(GcDbObjectIdArray&, GcDbIntArray&) const = 0;

  GcDb::DuplicateRecordCloning mergeStyle() const override = 0;

  enum
  {
    kNoOperation = 0,
    kEraseAllowed = 0x01,
    kCloningAllowed = 0x80,
    kAllButCloningAllowed = 0x01,
    kAllAllowedBits = 0x81,
    kMergeIgnore = 0,
    kMergeReplace = 0x100,
    kMergeMangleName = 0x200,
    kDisableProxyWarning = 0x400
  };
};

class GCDB_PORT GSOFT_NO_VTABLE GcDbProxyEntity : public GcDbEntity
{
public:
  GCRX_DECLARE_MEMBERS(GcDbProxyEntity);

  ~GcDbProxyEntity(){};

  virtual int proxyFlags() const = 0;
  virtual const GCHAR* originalClassName() const = 0;
  virtual const GCHAR* originalDxfName() const = 0;
  virtual const GCHAR* applicationDescription() const = 0;
  virtual Gcad::ErrorStatus getReferences(GcDbObjectIdArray&, GcDbIntArray&) const = 0;

  enum GraphicsMetafileType
  {
    kNoMetafile = 0,
    kBoundingBox = 1,
    kFullGraphics = 2
  };

  virtual GcDbProxyEntity::GraphicsMetafileType graphicsMetafileType() const = 0;

  enum
  {
    kNoOperation = 0,
    kEraseAllowed = 0x1,
    kTransformAllowed = 0x2,
    kColorChangeAllowed = 0x4,
    kLayerChangeAllowed = 0x8,
    kLinetypeChangeAllowed = 0x10,
    kLinetypeScaleChangeAllowed = 0x20,
    kVisibilityChangeAllowed = 0x40,
    kCloningAllowed = 0x80,
    kLineWeightChangeAllowed = 0x100,
    kPlotStyleNameChangeAllowed = 0x200,
    kAllButCloningAllowed = 0x37F,
    kAllAllowedBits = 0xBFF,
    kDisableProxyWarning = 0x400,
    kMaterialChangeAllowed = 0x800,
  };
};

#pragma pack(pop)