require "Common.Prerequisites"
connectServer()

--[[
local g_debugMode = true
if true == g_debugMode then
    package.cpath = string.format("%s;%s/?.dll", package.cpath, "E:/Self/Self/unity/unitygame/Client/Assets/Plugins/x86_64")
    local initconnection = require "debugger"
    initconnection("127.0.0.1", "10000", "luaidekey", "debugger.transport.luasocket", "win", "E:/Self/Self/unity/unitygame/Client/Assets/Prefabs/Resources/LuaScript")
end
]]

--[[

mime   = require("mime")

function nullRet()

    if mime == nil then
        return 0
    else 
        if mime.encode == nil then
            return 1
        end
    end
    return 2
end
]]

function luaFunc(i)
    a = i + 100
    print(a)
    return a
end

function addVarArg(...)
    print(3)
    local arg = {...}
    local n = table.getn(arg)
    --print(n)
    print(2)
        return arg[1] + arg[2]
end

--addVarArg(10, 20)

--require("TestLua")

aaa = luaFunc(10)
print(aaa)