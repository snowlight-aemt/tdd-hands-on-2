package accounting.test;


import accounting.Order;
import autoparams.customization.CompositeCustomizer;
import autoparams.customization.Customizer;
import autoparams.customization.InstanceFieldWriter;

import java.awt.*;

public class AccountingCustomizer extends CompositeCustomizer {
    public AccountingCustomizer() {
        super(new InstanceFieldWriter(Order.class), new EmptyShipReaderCustomizer());
    }
}
