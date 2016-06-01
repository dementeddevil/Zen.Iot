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

DEFINE_GUID (GUID_DEVINTERFACE_USB2514HubDriver,
    0x9aa33dbf,0x9851,0x4bc2,0x91,0xba,0x0b,0xd8,0x6a,0x12,0x0c,0x90);
// {9aa33dbf-9851-4bc2-91ba-0bd86a120c90}
