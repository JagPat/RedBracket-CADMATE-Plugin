﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef __GXBOILER_H_
#define __GXBOILER_H_

#include "gsoft.h"
#ifndef AXAUTOEXP
#  ifdef AXAUTO_DLL
#    define AXAUTOEXP __declspec(dllexport)
#  else
#    define AXAUTOEXP __declspec(dllimport)
#  endif
#endif

#include "gxobjref.h"

#ifdef _GSOFT_WINDOWS_
DEFINE_GUID(IID_IGcadBaseObject, 0xd857affd, 0x072a, 0x4548, 0x9b, 0x18, 0x28, 0xe0, 0xf4, 0x2d, 0x3d, 0x57);

#  undef INTERFACE
#  define INTERFACE IGcadBaseObject

interface DECLSPEC_UUID("D857AFFD-072A-4548-9B18-28E0F42D3D57") IGcadBaseObject : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID FAR * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(SetObjectId)
  (THIS_ GcDbObjectId & objId, GcDbObjectId ownerId = GcDbObjectId::kNull, GCHAR* keyName = NULL) PURE;
  STDMETHOD(GetObjectId)(THIS_ GcDbObjectId * objId) PURE;
  STDMETHOD(Clone)(THIS_ GcDbObjectId ownerId, LPUNKNOWN * pUnkClone) PURE;
  STDMETHOD(GetClassID)(THIS_ CLSID & clsid) PURE;
  STDMETHOD(NullObjectId)(THIS) PURE;
  STDMETHOD(OnModified)(THIS) PURE;
};

typedef IGcadBaseObject* LPGCADBASEOBJECT;

DEFINE_GUID(IID_IGcadBaseObject2, 0x867335a8, 0xd88d, 0x4ff3, 0xbb, 0xff, 0x8d, 0x38, 0x16, 0xac, 0xc8, 0xc8);

interface DECLSPEC_UUID("867335A8-D88D-4FF3-BBFF-8D3816ACC8C8") IGcadBaseObject2 : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID FAR * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(ForceDbResident)(THIS_ VARIANT_BOOL * forceDbResident) PURE;
  STDMETHOD(AddToDb)
  (THIS_ GcDbObjectId & objId, GcDbObjectId ownerId = GcDbObjectId::kNull, GCHAR* keyName = NULL) PURE;
  STDMETHOD(CreateObject)(THIS_ GcDbObjectId ownerId = GcDbObjectId::kNull, GCHAR* keyName = NULL) PURE;
  STDMETHOD(ObjectExists)(THIS_ VARIANT_BOOL * objectExists) PURE;
  STDMETHOD(SetObject)(THIS_ GcDbObject * &pObj) PURE;
  STDMETHOD(GetObject)(THIS_ GcDbObject * &pObj) PURE;
  STDMETHOD(OnModified)(THIS_ DISPID = DISPID_UNKNOWN) PURE;
};

typedef IGcadBaseObject2* LPGCADBASEOBJECT2;

DEFINE_GUID(IID_IRetrieveApplication, 0x93065461, 0x8f52, 0x4105, 0x9e, 0xe8, 0x90, 0x2c, 0xce, 0x9a, 0xc6, 0xbf);

interface DECLSPEC_UUID("93065461-8F52-4105-9EE8-902CCE9AC6BF") IRetrieveApplication : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(SetApplicationObject)(THIS_ LPDISPATCH pAppDisp) PURE;
  STDMETHOD(GetApplicationObject)(THIS_ LPDISPATCH * pAppDisp) PURE;
};

typedef IRetrieveApplication* LPRETRIEVEAPPLICATION;

DEFINE_GUID(IID_IGcadBaseDatabase, 0x628f20a2, 0x963c, 0x43a8, 0xa8, 0xdc, 0x62, 0x56, 0x8d, 0x50, 0xe7, 0x0b);

interface DECLSPEC_UUID("628F20A2-963C-43A8-A8DC-62568D50E70B") IGcadBaseDatabase : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(SetDatabase)(THIS_ GcDbDatabase * &pDb) PURE;
  STDMETHOD(GetDatabase)(THIS_ GcDbDatabase * *pDb) PURE;
  STDMETHOD(GetClassID)(THIS_ CLSID & clsid) PURE;
};

typedef IGcadBaseDatabase* LPGCADBASEDATABASE;

DEFINE_GUID(IID_IIMetaColorSuppressor, 0x3aafa112, 0x8dff, 0x422d, 0x99, 0x6c, 0x2a, 0x46, 0x7d, 0x57, 0x8a, 0xc8);

interface DECLSPEC_UUID("3AAFA112-8DFF-422D-996C-2A467D578AC8") IMetaColorSuppressor : public IUnknown
{
  STDMETHOD(GetSuppressor)(THIS_ bool* val) PURE;
  STDMETHOD(SetSuppressor)(THIS_ bool val) PURE;
};

typedef IMetaColorSuppressor* LPMETACOLORSUPPRESSOR;

DEFINE_GUID(IID_IGcadGcCmColorAccess, 0x195006b6, 0x14ef, 0x4eb2, 0x8f, 0x61, 0x4d, 0x7f, 0x6b, 0x07, 0x5f, 0xd0);

#  undef INTERFACE
#  define INTERFACE IGcadGcCmColorAccess

interface DECLSPEC_UUID("195006B6-14EF-4EB2-8F61-4D7F6B075FD0") IGcadGcCmColorAccess : public IUnknown
{
  STDMETHOD(GetGcCmColor)(THIS_ GcCmColor * clr) PURE;
  STDMETHOD(SetGcCmColor)(THIS_ GcCmColor * clr) PURE;
};

typedef IGcadGcCmColorAccess* LPGCADGCCMCOLORACCESS;

DEFINE_GUID(IID_IGcadBaseSubEntity, 0x5306323d, 0xcf0a, 0x42d5, 0x90, 0x5d, 0x73, 0x58, 0x7e, 0x82, 0x38, 0x36);

#  undef INTERFACE
#  define INTERFACE IGcadBaseSubEntity

interface DECLSPEC_UUID("5306323D-CF0A-42D5-905D-73587E823836") IGcadBaseSubEntity : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID FAR * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(OnModified)() PURE;
  STDMETHOD(SetFullSubEntPath)(THIS_ GcDbFullSubentPath & subentPath) PURE;
  STDMETHOD(GetFullSubEntPath)(THIS_ GcDbFullSubentPath * subentPath) PURE;
  STDMETHOD(GetClassID)(THIS_ CLSID & clsid) PURE;
};

typedef IGcadBaseSubEntity* LPGCADSUBENTITY;

HRESULT AXAUTOEXP GcAxErase(GcDbObjectId& objId);

HRESULT AXAUTOEXP GcAxSetXData(GcDbObjectId& objId, VARIANT type, VARIANT data);
HRESULT AXAUTOEXP GcAxGetXData(GcDbObjectId& objId, BSTR bstrName, VARIANT* type, VARIANT* data);

HRESULT AXAUTOEXP GcAxGetObjectName(GcDbObjectId& objId, BSTR* pName);

HRESULT AXAUTOEXP GcAxGetHandle(GcDbObjectId& objId, BSTR* pHandle);

HRESULT AXAUTOEXP GcAxGetColor(GcDbObjectId& objId, GcColor* pColor);
HRESULT AXAUTOEXP GcAxPutColor(GcDbObjectId& objId, GcColor color);
HRESULT AXAUTOEXP GcAxGetTrueColor(GcDbObjectId& objId, IGcadGcCmColor** pColor);
HRESULT AXAUTOEXP GcAxPutTrueColor(GcDbObjectId& objId, IGcadGcCmColor* color);

HRESULT AXAUTOEXP GcAxGetLayer(GcDbObjectId& objId, BSTR* pLayer);
HRESULT AXAUTOEXP GcAxPutLayer(GcDbObjectId& objId, BSTR layer);

HRESULT AXAUTOEXP GcAxGetLinetype(GcDbObjectId& objId, BSTR* pLinetype);
HRESULT AXAUTOEXP GcAxPutLinetype(GcDbObjectId& objId, BSTR linetype);

HRESULT AXAUTOEXP GcAxGetTransparency(GcDbObjectId& objId, BSTR* pTransparency);
HRESULT AXAUTOEXP GcAxPutTransparency(GcDbObjectId& objId, BSTR transparency);

HRESULT AXAUTOEXP GcAxGetShadowDisplay(GcDbObjectId& objId, GcShadowDisplayType* pShadowDisplay);
HRESULT AXAUTOEXP GcAxPutShadowDisplay(GcDbObjectId& objId, GcShadowDisplayType shadowDisplay);
HRESULT AXAUTOEXP GcAxEnableShadowDisplay(GcDbObjectId& objId, bool* pEnabled);

HRESULT AXAUTOEXP GcAxGetMaterial(GcDbObjectId& objId, BSTR* pMaterial);
HRESULT AXAUTOEXP GcAxPutMaterial(GcDbObjectId& objId, BSTR material);

HRESULT AXAUTOEXP GcAxGetLinetypeScale(GcDbObjectId& objId, double* pLinetypeScale);
HRESULT AXAUTOEXP GcAxPutLinetypeScale(GcDbObjectId& objId, double linetypeScale);

HRESULT AXAUTOEXP GcAxGetVisible(GcDbObjectId& objId, VARIANT_BOOL* pVisible);
HRESULT AXAUTOEXP GcAxPutVisible(GcDbObjectId& objId, VARIANT_BOOL visible);

HRESULT AXAUTOEXP GcAxArrayPolar(
  GcDbObjectId& objId, LPDISPATCH pAppDisp, int numObjs, double fillAngle, VARIANT basePoint, VARIANT* pArrayObjs);

HRESULT AXAUTOEXP GcAxArrayRectangular(GcDbObjectId& objId,
                                       LPDISPATCH pAppDisp,
                                       int numRows,
                                       int numCols,
                                       int numLvls,
                                       double disRows,
                                       double disCols,
                                       double disLvls,
                                       VARIANT* pArrayObjs);

HRESULT AXAUTOEXP GcAxHighlight(GcDbObjectId& objId, VARIANT_BOOL bHighlight);

HRESULT AXAUTOEXP GcAxCopy(GcDbObjectId& objId,
                           LPDISPATCH pAppDisp,
                           LPDISPATCH* pCopyObj,
                           GcDbObjectId ownerId = GcDbObjectId::kNull);
HRESULT AXAUTOEXP GcAxMove(GcDbObjectId& objId, VARIANT fromPoint, VARIANT toPoint);
HRESULT AXAUTOEXP GcAxRotate(GcDbObjectId& objId, VARIANT basePoint, double rotationAngle);
HRESULT AXAUTOEXP GcAxRotate3D(GcDbObjectId& objId, VARIANT point1, VARIANT point2, double rotationAngle);
HRESULT AXAUTOEXP
GcAxMirror(GcDbObjectId& objId, LPDISPATCH pAppDisp, VARIANT point1, VARIANT point2, LPDISPATCH* pMirrorObj);
HRESULT AXAUTOEXP GcAxMirror3D(
  GcDbObjectId& objId, LPDISPATCH pAppDisp, VARIANT point1, VARIANT point2, VARIANT point3, LPDISPATCH* pMirrorObj);
HRESULT AXAUTOEXP GcAxScaleEntity(GcDbObjectId& objId, VARIANT basePoint, double scaleFactor);
HRESULT AXAUTOEXP GcAxTransformBy(GcDbObjectId& objId, VARIANT transMatrix);
HRESULT AXAUTOEXP GcAxUpdate(GcDbObjectId& objId);

HRESULT AXAUTOEXP GcAxGetBoundingBox(GcDbObjectId& objId, VARIANT* minPoint, VARIANT* maxPoint);

HRESULT AXAUTOEXP GcAxIntersectWith(GcDbObjectId& objId, LPDISPATCH pEntity, GcExtendOption option, VARIANT* intPoints);

HRESULT AXAUTOEXP GcAxGetPlotStyleName(GcDbObjectId& objId, BSTR* plotStyleName);
HRESULT AXAUTOEXP GcAxPutPlotStyleName(GcDbObjectId& objId, BSTR plotStyleName);

HRESULT AXAUTOEXP GcAxGetLineWeight(GcDbObjectId& objId, GCAD_LWEIGHT* lineWeight);
HRESULT AXAUTOEXP GcAxPutLineWeight(GcDbObjectId& objId, GCAD_LWEIGHT lineWeight);

HRESULT AXAUTOEXP GcAxGetHyperlinks(GcDbObjectId& objId, LPDISPATCH pAppDisp, IGcadHyperlinks** pHyperlinks);

HRESULT AXAUTOEXP GcAxHasExtensionDictionary(GcDbObjectId& objId, VARIANT_BOOL* bHasDictionary);
HRESULT AXAUTOEXP GcAxGetExtensionDictionary(GcDbObjectId& objId,
                                             LPDISPATCH pAppDisp,
                                             IGcadDictionary** pExtDictionary);

HRESULT AXAUTOEXP GcAxGetDatabase(GcDbDatabase* pDb, LPDISPATCH pAppDisp, LPDISPATCH* pDisp);

HRESULT AXAUTOEXPGcAxGetOwnerId(GcDbObjectId& objId, Gsoft::IntDbId* pOwnerId);

#  if defined(_WIN64) || defined(_GC64)
Gsoft::Int32 AXAUTOEXP GcAxGetObjectId32(const Gsoft::IntDbId& objId);
Gsoft::IntDbId AXAUTOEXP GcAxGetObjectId64(const Gsoft::Int32& objId);
#  endif

HRESULT AXAUTOEXP RaiseGcAXException(REFGUID iid, HRESULT hRes, DWORD dwException, ...);
HRESULT AXAUTOEXP RaiseGrxException(REFGUID iid, HRESULT hRes, Gcad::ErrorStatus es);

HRESULT AXAUTOEXP GcAxGetIUnknownOfObject(LPUNKNOWN* ppUnk, GcDbObjectId& objId, LPDISPATCH pApp);
HRESULT AXAUTOEXP GcAxGetIUnknownOfObject(LPUNKNOWN* ppUnk, GcDbObject* pObj, LPDISPATCH pApp);

HRESULT AXAUTOEXP GcAxErase(GcAxObjectRef& objRef);

HRESULT AXAUTOEXP GcAxSetXData(GcAxObjectRef& objRef, VARIANT type, VARIANT data);
HRESULT AXAUTOEXP GcAxGetXData(GcAxObjectRef& objRef, BSTR bstrName, VARIANT* type, VARIANT* data);

HRESULT AXAUTOEXP GcAxGetObjectName(GcAxObjectRef& objRef, BSTR* pName);

HRESULT AXAUTOEXP GcAxGetHandle(GcAxObjectRef& objRef, BSTR* pHandle);

HRESULT AXAUTOEXP GcAxGetColor(GcAxObjectRef& objRef, GcColor* pColor);
HRESULT AXAUTOEXP GcAxPutColor(GcAxObjectRef& objRef, GcColor color);
HRESULT AXAUTOEXP GcAxGetTrueColor(GcAxObjectRef& objRef, IGcadGcCmColor** pColor);
HRESULT AXAUTOEXP GcAxPutTrueColor(GcAxObjectRef& objRef, IGcadGcCmColor* color);

HRESULT AXAUTOEXP GcAxGetLayer(GcAxObjectRef& objRef, BSTR* pLayer);
HRESULT AXAUTOEXP GcAxPutLayer(GcAxObjectRef& objRef, BSTR layer);

HRESULT AXAUTOEXP GcAxGetLinetype(GcAxObjectRef& objRef, BSTR* pLinetype);
HRESULT AXAUTOEXP GcAxPutLinetype(GcAxObjectRef& objRef, BSTR linetype);

HRESULT AXAUTOEXP GcAxGetTransparency(GcAxObjectRef& objRef, BSTR* pTransparency);
HRESULT AXAUTOEXP GcAxPutTransparency(GcAxObjectRef& objRef, BSTR transparency);

HRESULT AXAUTOEXP GcAxGetMaterial(GcAxObjectRef& objRef, BSTR* pMaterial);
HRESULT AXAUTOEXP GcAxPutMaterial(GcAxObjectRef& objRef, BSTR material);

HRESULT AXAUTOEXP GcAxGetLinetypeScale(GcAxObjectRef& objRef, double* pLinetypeScale);
HRESULT AXAUTOEXP GcAxPutLinetypeScale(GcAxObjectRef& objRef, double linetypeScale);

HRESULT AXAUTOEXP GcAxGetShadowDisplay(GcAxObjectRef& objRef, GcShadowDisplayType* pShadowDisplay);
HRESULT AXAUTOEXP GcAxPutShadowDisplay(GcAxObjectRef& objRef, GcShadowDisplayType shadowDisplay);
HRESULT AXAUTOEXP GcAxEnableShadowDisplay(GcAxObjectRef& objRef, bool* pEnabled);

HRESULT AXAUTOEXP GcAxGetVisible(GcAxObjectRef& objRef, VARIANT_BOOL* pVisible);
HRESULT AXAUTOEXP GcAxPutVisible(GcAxObjectRef& objRef, VARIANT_BOOL visible);

HRESULT AXAUTOEXP GcAxArrayPolar(
  GcAxObjectRef& objRef, LPDISPATCH pAppDisp, int numObjs, double fillAngle, VARIANT basePoint, VARIANT* pArrayObjs);

HRESULT AXAUTOEXP GcAxArrayRectangular(GcAxObjectRef& objRef,
                                       LPDISPATCH pAppDisp,
                                       int numRows,
                                       int numCols,
                                       int numLvls,
                                       double disRows,
                                       double disCols,
                                       double disLvls,
                                       VARIANT* pArrayObjs);

HRESULT AXAUTOEXP GcAxHighlight(GcAxObjectRef& objRef, VARIANT_BOOL bHighlight);

HRESULT AXAUTOEXP GcAxCopy(GcAxObjectRef& objRef,
                           LPDISPATCH pAppDisp,
                           LPDISPATCH* pCopyObj,
                           GcDbObjectId ownerId = GcDbObjectId::kNull);
HRESULT AXAUTOEXP GcAxMove(GcAxObjectRef& objRef, VARIANT fromPoint, VARIANT toPoint);
HRESULT AXAUTOEXP GcAxRotate(GcAxObjectRef& objRef, VARIANT basePoint, double rotationAngle);
HRESULT AXAUTOEXP GcAxRotate3D(GcAxObjectRef& objRef, VARIANT point1, VARIANT point2, double rotationAngle);
HRESULT AXAUTOEXP
GcAxMirror(GcAxObjectRef& objRef, LPDISPATCH pAppDisp, VARIANT point1, VARIANT point2, LPDISPATCH* pMirrorObj);
HRESULT AXAUTOEXP GcAxMirror3D(
  GcAxObjectRef& objRef, LPDISPATCH pAppDisp, VARIANT point1, VARIANT point2, VARIANT point3, LPDISPATCH* pMirrorObj);
HRESULT AXAUTOEXP GcAxScaleEntity(GcAxObjectRef& objRef, VARIANT basePoint, double scaleFactor);
HRESULT AXAUTOEXP GcAxTransformBy(GcAxObjectRef& objRef, VARIANT transMatrix);
HRESULT AXAUTOEXP GcAxUpdate(GcAxObjectRef& objRef);

HRESULT AXAUTOEXP GcAxGetBoundingBox(GcAxObjectRef& objRef, VARIANT* minPoint, VARIANT* maxPoint);

HRESULT AXAUTOEXP GcAxIntersectWith(GcAxObjectRef& objRef,
                                    LPDISPATCH pEntity,
                                    GcExtendOption option,
                                    VARIANT* intPoints);

HRESULT AXAUTOEXP GcAxGetPlotStyleName(GcAxObjectRef& objRef, BSTR* plotStyleName);
HRESULT AXAUTOEXP GcAxPutPlotStyleName(GcAxObjectRef& objRef, BSTR plotStyleName);

HRESULT AXAUTOEXP GcAxGetLineWeight(GcAxObjectRef& objRef, GCAD_LWEIGHT* lineWeight);
HRESULT AXAUTOEXP GcAxPutLineWeight(GcAxObjectRef& objRef, GCAD_LWEIGHT lineWeight);

HRESULT AXAUTOEXP GcAxGetHyperlinks(GcAxObjectRef& objRef, LPDISPATCH pAppDisp, IGcadHyperlinks** pHyperlinks);

HRESULT AXAUTOEXP GcAxHasExtensionDictionary(GcAxObjectRef& objRef, VARIANT_BOOL* bHasDictionary);
HRESULT AXAUTOEXP GcAxGetExtensionDictionary(GcAxObjectRef& objRef,
                                             LPDISPATCH pAppDisp,
                                             IGcadDictionary** pExtDictionary);

HRESULT AXAUTOEXP GcAxGetOwnerId(GcAxObjectRef& objRef, Gsoft::IntDbId* pOwnerId);

HRESULT AXAUTOEXP GcAxGet_UIsolineDensity(GcDbObjectId& objId, long* density);
HRESULT AXAUTOEXP GcAxPut_UIsolineDensity(GcDbObjectId& objId, long density);
HRESULT AXAUTOEXP GcAxGet_VIsolineDensity(GcDbObjectId& objId, long* density);
HRESULT AXAUTOEXP GcAxPut_VIsolineDensity(GcDbObjectId& objId, long density);

HRESULT AXAUTOEXP GcAxGet_WireframeType(GcDbObjectId& objId, GcWireframeType* type);
HRESULT AXAUTOEXP GcAxPut_WireframeType(GcDbObjectId& objId, GcWireframeType type);

HRESULT AXAUTOEXP GcAxGetMaintainAssociativity(GcDbObjectId& objId, int* maintainAssoc);
HRESULT AXAUTOEXP GcAxPutMaintainAssociativity(GcDbObjectId& objId, int maintainAssoc);

HRESULT AXAUTOEXP GcAxGetShowAssociativity(GcDbObjectId& objId, VARIANT_BOOL* bEnabled);
HRESULT AXAUTOEXP GcAxPutShowAssociativity(GcDbObjectId& objId, VARIANT_BOOL bEnabled);

HRESULT AXAUTOEXP GcAxGetEdgeExtensionDistances(GcDbObjectId& objId, VARIANT* extDistances);
HRESULT AXAUTOEXP GcAxPutEdgeExtensionDistances(GcDbObjectId& objId, VARIANT extDistances);

HRESULT AXAUTOEXP GcAxGetSurfTrimAssociativity(GcDbObjectId& objId, VARIANT* associative);
HRESULT AXAUTOEXP GcAxPutSurfTrimAssociativity(GcDbObjectId& objId, VARIANT associative);

#else

DEFINE_GUID(IID_IGcadBaseObject, 0xd857affd, 0x072a, 0x4548, 0x9b, 0x18, 0x28, 0xe0, 0xf4, 0x2d, 0x3d, 0x57);

#  undef INTERFACE
#  define INTERFACE IGcadBaseObject

interface IGcadBaseObject : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID FAR * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(SetObjectId)
  (THIS_ GcDbObjectId & objId, GcDbObjectId ownerId = GcDbObjectId::kNull, GCHAR* keyName = NULL) PURE;
  STDMETHOD(GetObjectId)(THIS_ GcDbObjectId * objId) PURE;
  STDMETHOD(Clone)(THIS_ GcDbObjectId ownerId, LPUNKNOWN * pUnkClone) PURE;
  STDMETHOD(GetClassID)(THIS_ CLSID & clsid) PURE;
  STDMETHOD(NullObjectId)(THIS) PURE;
  STDMETHOD(OnModified)(THIS) PURE;
};

typedef IGcadBaseObject* LPGCADBASEOBJECT;

DEFINE_GUID(IID_IGcadBaseObject2, 0x867335a8, 0xd88d, 0x4ff3, 0xbb, 0xff, 0x8d, 0x38, 0x16, 0xac, 0xc8, 0xc8);

interface IGcadBaseObject2 : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID FAR * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(ForceDbResident)(THIS_ VARIANT_BOOL * forceDbResident) PURE;
  STDMETHOD(AddToDb)
  (THIS_ GcDbObjectId & objId, GcDbObjectId ownerId = GcDbObjectId::kNull, GCHAR* keyName = NULL) PURE;
  STDMETHOD(CreateObject)(THIS_ GcDbObjectId ownerId = GcDbObjectId::kNull, GCHAR* keyName = NULL) PURE;
  STDMETHOD(ObjectExists)(THIS_ VARIANT_BOOL * objectExists) PURE;
  STDMETHOD(SetObject)(THIS_ GcDbObject * &pObj) PURE;
  STDMETHOD(GetObject)(THIS_ GcDbObject * &pObj) PURE;
  STDMETHOD(OnModified)(THIS_ DISPID = DISPID_UNKNOWN) PURE;
};

typedef IGcadBaseObject2* LPGCADBASEOBJECT2;

DEFINE_GUID(IID_IGcadBaseDatabase, 0x628f20a2, 0x963c, 0x43a8, 0xa8, 0xdc, 0x62, 0x56, 0x8d, 0x50, 0xe7, 0x0b);

interface IGcadBaseDatabase : public IUnknown
{
  STDMETHOD(QueryInterface)(THIS_ REFIID riid, LPVOID FAR * ppvObj) PURE;
  STDMETHOD_(ULONG, AddRef)(THIS) PURE;
  STDMETHOD_(ULONG, Release)(THIS) PURE;

  STDMETHOD(SetDatabase)(THIS_ GcDbDatabase * &pDb) PURE;
  STDMETHOD(GetDatabase)(THIS_ GcDbDatabase * *pDb) PURE;
  STDMETHOD(GetClassID)(THIS_ CLSID & clsid) PURE;
};

typedef IGcadBaseDatabase* LPGCADBASEDATABASE;

#endif

#endif
