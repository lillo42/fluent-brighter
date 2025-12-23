{

  description = ".NET 9 Development Environment";
  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs/nixos-unstable";
    flake-utils.url = "github:numtide/flake-utils";
  };

  outputs =
    { nixpkgs, flake-utils, ... }:
    flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = import nixpkgs {
          inherit system;
          # Allow unfree packages if you use specific proprietary dotnet tools
          config.allowUnfree = true;
        };

        # Define the .NET SDK version you want to use
        dotnetSdk = pkgs.dotnetCorePackages.sdk_9_0-bin;
      in
      {
        devShells.default = pkgs.mkShell {
          # Tools included in the environment
          packages = [
            pkgs.zsh
            dotnetSdk

            # --- Optional: IDE Tooling ---
            pkgs.netcoredbg # Debugger for .NET Core
            pkgs.roslyn-ls # LSP for VS Code / Emacs / Vim
          ];

          # Environment variables
          # 1. Essential: Tell dotnet tools where to find the SDK
          DOTNET_ROOT = "${dotnetSdk}";

          # 3. Add .NET tools to PATH (e.g., dotnet-ef installed locally)
          #          shellHook = ''
          #          export PATH="$PATH:$HOME/.dotnet/tools"
          #          '';
        };
      }
    );
}
