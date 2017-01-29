/*++

Module Name:

    public.h

Abstract:

    This module contains the common declarations shared by driver
    and user applications.

Environment:

    user and kernel

--*/

//
// Define an Interface Guid so that app can find the device and talk to it.
//

DEFINE_GUID (GUID_DEVINTERFACE_Ds1339ClockDriver,
    0x97615868,0x7b86,0x447d,0x99,0x83,0x0a,0x32,0x38,0xae,0xae,0x61);
// {97615868-7b86-447d-9983-0a3238aeae61}
