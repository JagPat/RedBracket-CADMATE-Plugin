﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef __DBSECURITY_H_
#define __DBSECURITY_H_

class GcDbDate;

enum
{
  SECURITYPARAMS_ENCRYPT_DATA = 0x00000001,
  SECURITYPARAMS_ENCRYPT_PROPS = 0x00000002,
  SECURITYPARAMS_SIGN_DATA = 0x00000010,
  SECURITYPARAMS_ADD_TIMESTAMP = 0x00000020
};

enum
{
  SECURITYPARAMS_ALGID_RC4 = 0x00006801
};

struct SecurityParams
{
  unsigned long cbSize;
  unsigned long ulFlags;

  wchar_t* wszPassword;
  unsigned long ulProvType;
  wchar_t* wszProvName;
  unsigned long ulAlgId;
  unsigned long ulKeyLength;

  wchar_t* wszCertSubject;
  wchar_t* wszCertIssuer;
  wchar_t* wszCertSerialNum;
  wchar_t* wszComment;
  wchar_t* wszTimeServer;
};

struct SignatureInfo
{
  unsigned long cbSize;

  unsigned long ulFlags;
  wchar_t* wszCertSubject;
  wchar_t* wszCertIssuer;
  wchar_t* wszSubjectEMail;
  wchar_t* wszCertSerialNum;
  wchar_t* wszX500;
  GcDbDate* pTimeCertNotBefore;
  GcDbDate* pTimeCertNotAfter;
  wchar_t* wszComment;
  wchar_t* wszTimeServer;
  GcDbDate* pTimestamp;
};

#endif