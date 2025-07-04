﻿/////////////////////////////////////////////////////////////////////////////////////////
//
// Please refer to "COPYRIGHT.md" for the relevant copyright statement of this software.
//
/////////////////////////////////////////////////////////////////////////////////////////
//
#ifndef GCAPDOCLOCKMODE_H
#define GCAPDOCLOCKMODE_H

#pragma pack(push, 8)
struct GcAp
{
  enum DocLockMode
  {
    kNone = 0x00,
    kAutoWrite = 0x01,
    kNotLocked = 0x02,
    kWrite = 0x04,
    kProtectedAutoWrite = 0x14,
    kRead = 0x20,
    kXWrite = 0x40
  };
};
#pragma pack(pop)

#endif